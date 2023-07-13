using System.Text.Json;
using Extensions;
using KickStreaming.Authentication;
using KickStreaming.Chat;
using KickStreaming.Models.CycleTls;
using KickStreaming.User;
using Microsoft.Extensions.Logging;

namespace KickStreaming; 

public class KickApi {

    internal readonly CycleTls CycleTls;
    internal readonly Logger   Logger;
    private  const    string   BaseUrl = "https://kick.com";
    public            string   Token  = "";
    
    // Modules
    public readonly KickAuthentication Authentication;
    public readonly KickChat           Chat;
    public readonly KickUser           User;

    public KickApi(Logger.Output loggerOutput = Logger.Output.File, Logger.Level loggerLevel = Logger.Level.Warning) {
        Logger   = new Logger(loggerOutput, loggerLevel);
        CycleTls = new CycleTls(LoggerFactory.Create(b => b.AddConsole()).CreateLogger<CycleTls>());

        Authentication = new KickAuthentication(this);
        Chat           = new KickChat(this);
        User           = new KickUser(this);
    }

    #region "Spoof Options"

    internal static CycleTlsRequestOptions GetSpoofOptions(string endpoint, int version, HttpMethod method, Dictionary<string, string> headers) =>
        new() {
            Ja3 = "771,4865-4867-4866-49195-49199-52393-52392-49196-49200-49162-49161-49171-49172-51-57-47-53-10,0-23-65281-10-11-35-16-5-51-43-13-45-28-21,29-23-24-25-256-257,0",
            UserAgent = "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:87.0) Gecko/20100101 Firefox/87.0",
            Url = $"${BaseUrl}/api/v{version}/{endpoint}",
            Method = method.Method,
            Headers = headers
        };
    
    internal static CycleTlsRequestOptions GetSpoofOptions(string endpoint, HttpMethod method, Dictionary<string, string> headers) =>
        new() {
            Ja3 = "771,4865-4867-4866-49195-49199-52393-52392-49196-49200-49162-49161-49171-49172-51-57-47-53-10,0-23-65281-10-11-35-16-5-51-43-13-45-28-21,29-23-24-25-256-257,0",
            UserAgent = "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:87.0) Gecko/20100101 Firefox/87.0",
            Url = $"${BaseUrl}/{endpoint}",
            Method = method.Method,
            Headers = headers
        };
    
    internal static CycleTlsRequestOptions PostSpoofOptions<T>(string endpoint, int version, T body, Dictionary<string, string> headers) =>
        new() {
            Ja3 = "771,4865-4867-4866-49195-49199-52393-52392-49196-49200-49162-49161-49171-49172-51-57-47-53-10,0-23-65281-10-11-35-16-5-51-43-13-45-28-21,29-23-24-25-256-257,0",
            UserAgent = "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:87.0) Gecko/20100101 Firefox/87.0",
            Url = $"${BaseUrl}/api/v{version}/{endpoint}",
            Method = HttpMethod.Post.Method,
            Body = JsonSerializer.Serialize(body, options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase}),
            Headers = headers
        };
    
    internal static CycleTlsRequestOptions PostSpoofOptions<T>(string endpoint, T body, Dictionary<string, string> headers) =>
        new() {
            Ja3 = "771,4865-4867-4866-49195-49199-52393-52392-49196-49200-49162-49161-49171-49172-51-57-47-53-10,0-23-65281-10-11-35-16-5-51-43-13-45-28-21,29-23-24-25-256-257,0",
            UserAgent = "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:87.0) Gecko/20100101 Firefox/87.0",
            Url = $"${BaseUrl}/{endpoint}",
            Method = HttpMethod.Post.Method,
            Body = JsonSerializer.Serialize(body, options: new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase}),
            Headers = headers
        };

    #endregion

}