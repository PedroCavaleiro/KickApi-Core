using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;
using KickStreaming.Models.CycleTls;
using Microsoft.Extensions.Logging;
using WebSocketSharper;

namespace KickStreaming; 

internal class CycleTls {
    
    private readonly ILogger<CycleTls> _logger;

    public TimeSpan DefaultTimeOut { get; set; } = TimeSpan.FromSeconds(100);

    private WebSocket WebSocketClient { get; set; } = null;


    private readonly object _lockQueue = new();

    private bool _isQueueSendRunning;

    private Queue<(CycleTlsRequest Request, TaskCompletionSource<CycleTlsResponse> RequestTCS)> RequestQueue { get; set; }
        = new();

    private ConcurrentDictionary<string, TaskCompletionSource<CycleTlsResponse>> SentRequests { get; set; }
        = new();

    private object _lockRequestCount = new();
    private int    RequestCount { get; set; } = 0;
    
    private CycleTlsRequestOptions DefaultRequestOptions { get; } = new() {
        Ja3 = "771,4865-4867-4866-49195-49199-52393-52392-49196-49200-49162-49161-49171-49172-51-57-47-53-10,0-23-65281-10-11-35-16-5-51-43-13-45-28-21,29-23-24-25-256-257,0",
        UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.54 Safari/537.36",
        Body            = "",
        Cookies         = new List<Cookie>(),
        DisableRedirect = null,
        HeaderOrder     = new List<string>(),
        Headers         = new Dictionary<string, string>(),
        Method          = "",
        OrderAsProvided = null,
        Proxy           = "",
        Timeout         = null,
        Url             = ""
    };

    internal CycleTls(ILogger<CycleTls> logger, int bypassPort = 9112) {
        _logger             = logger;
        StartClient(bypassPort);
    }
    
    private void StartClient(int port) {
        var ws = new WebSocket(_logger, "ws://localhost:" + port, false);

        ws.OnMessage += (_, ea) => {
            var response = JsonSerializer.Deserialize<CycleTlsResponse>(ea.Data);
            if (!SentRequests.TryRemove(response?.RequestID ?? string.Empty, out var requestTcSource)) return;
            if (response != null)
                requestTcSource.TrySetResult(response);
        };

        ws.OnError += (_, ea) => {
            ws.Close();
            foreach (var requestPair in SentRequests)
                requestPair.Value.TrySetException(new Exception("Error in WebSocket connection.", ea.Exception));

            SentRequests.Clear();
            Task.Delay(100).ContinueWith((t) => StartClient(port));
        };

        ws.Connect();

        WebSocketClient = ws;
    }
    
    public async Task<CycleTlsResponse> SendAsync(HttpMethod httpMethod, string url) => 
        await SendAsync(httpMethod, url, DefaultTimeOut);
    
    public async Task<CycleTlsResponse> SendAsync(HttpMethod httpMethod, string url, TimeSpan timeout) =>
        await SendAsync(
            new CycleTlsRequestOptions {
                Url    = url,
                Method = httpMethod.Method
            }, timeout
        );
    
    public async Task<CycleTlsResponse> SendAsync(CycleTlsRequestOptions cycleTlsRequestOptions) =>
        await SendAsync(cycleTlsRequestOptions, DefaultTimeOut);
    
    public Task<CycleTlsResponse> SendAsync(CycleTlsRequestOptions cycleTlsRequestOptions, TimeSpan timeout) {
        if (WebSocketClient == null)
            throw new InvalidOperationException("WebSocket client is not initialized.");

        var tcs = new TaskCompletionSource<CycleTlsResponse>();
        var cancelSource = new CancellationTokenSource(timeout);
        cancelSource.Token.Register(() => tcs.TrySetException(new TimeoutException($"No response after {timeout.TotalSeconds} seconds.")));

        var request = CreateRequest(cycleTlsRequestOptions);

        lock (_lockQueue) {
            RequestQueue.Enqueue((request, tcs));
            if (_isQueueSendRunning) return tcs.Task;
            _isQueueSendRunning = true;
            Task.Run(QueueSendAsync, cancelSource.Token);
        }

        return tcs.Task;
    }
    
    private CycleTlsRequest CreateRequest(CycleTlsRequestOptions cycleTlsRequestOptions) {
        var optionsCopy = new CycleTlsRequestOptions();
        foreach (var propertyInfo in typeof(CycleTlsRequestOptions).GetProperties()) {
            var defaultOption = propertyInfo.GetValue(DefaultRequestOptions);
            var customOption  = propertyInfo.GetValue(cycleTlsRequestOptions);
            propertyInfo.SetValue(optionsCopy, customOption ?? defaultOption);
        }

        foreach (var cookie in optionsCopy.Cookies.Where(c => c.Expires == default))
            cookie.Expires = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();

        int requestIndex;
        lock (_lockRequestCount)
            requestIndex = ++RequestCount;

        var request = new CycleTlsRequest {
            RequestId = $"{requestIndex}:{DateTime.Now}:{optionsCopy.Url}",
            Options   = optionsCopy
        };

        return request;
    }
    
    private async Task QueueSendAsync() {
        while (true) {
            if (WebSocketClient == null)
                throw new InvalidOperationException("Critical error. For some reason WebSocket client is not initialized. " +
                                                    "Probably, you should not see this exception");

            if (!await ClientRestartCheckDelay()) return;

            CycleTlsRequest                        request;
            TaskCompletionSource<CycleTlsResponse> requestTcs;
            lock (_lockQueue) {
                if (!RequestQueue.Any()) {
                    _isQueueSendRunning = false;
                    return;
                }
                (request, requestTcs) = RequestQueue.Dequeue();
            }

            SentRequests.TryAdd(request.RequestId, requestTcs);

            var jsonRequestData = JsonSerializer.Serialize(request);

            WebSocketClient.SendAsync(jsonRequestData, (isCompleted) => {
                if (isCompleted) return;
                requestTcs.TrySetException(new Exception("Error in WebSocket connection."));
                SentRequests.TryRemove(request.RequestId, out _);
            });
        }
    }
    
    private async Task<bool> ClientRestartCheckDelay() {
        // Wait max 5000 milliseconds while server or client restarts
        var       attempts    = 0;
        const int maxAttempts = 50;
        const int delay       = 100;
        
        while (!WebSocketClient.IsAlive && attempts < 50) {
            await Task.Delay(delay);
            attempts++;
        }

        if (WebSocketClient.IsAlive) return true;
        lock (_lockQueue) {
            while (RequestQueue.Any()) {
                RequestQueue.Dequeue().RequestTCS
                            .TrySetException(new Exception($"Critical error. " +
                                                           $"WebSocket connection was not established after {maxAttempts * delay} milliseconds."));
            }
            _isQueueSendRunning = false;
            return false;
        }
    }

}