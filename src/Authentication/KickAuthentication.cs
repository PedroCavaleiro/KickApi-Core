using System.Text.Json;
using Extensions;
using KickStreaming.Authentication.Models;
using WebSocketSharper;

namespace KickStreaming.Authentication;

public class KickAuthentication {

    private readonly KickApi _kickApi;

    internal KickAuthentication(KickApi kickApi) => _kickApi = kickApi;

    /// <summary>
    /// Gets the login token for a Kick user 
    /// </summary>
    /// <remarks>2 Factor Authentication is not yet supported by the library, but it is by the API</remarks>
    /// <param name="password">User's password</param>
    /// <param name="username">(optional) The username of the user</param>
    /// <param name="email">(optional) The email of the user</param>
    /// <remarks>Username and/or Email must be passed</remarks>
    /// <returns>The login token</returns>
    public async Task<string> Login(string password, string username = "", string email = "") {

        var tokenProviderRequest = await _kickApi.CycleTls.SendAsync(KickApi.GetSpoofOptions("kick-token-provider", HttpMethod.Get, new Dictionary<string, string>()));
        if (tokenProviderRequest.Status != 200) {
            _kickApi.Logger.Log<KickApi>($"[API Error][Kick] Error on endpoint {KickApi.GetSpoofOptions("kick-token-provider", HttpMethod.Get, new Dictionary<string, string>()).Url.Replace("https://", "")}", Logger.Level.Warning);
            return string.Empty;
        }

        var tokenProvider = JsonSerializer.Deserialize<TokenProvider>(tokenProviderRequest.Body);
        
        var request = new Dictionary<string, object> {
            { "email", username.IsNullOrEmpty() ? email : username },
            { "password", password },
            { "isMobileRequest", true },
            { tokenProvider?.NameFieldName ?? string.Empty, "" },
            { tokenProvider?.ValidFromFieldName ?? string.Empty, tokenProvider?.EncryptedValidFrom ?? string.Empty }
        };

        var loginRequest = await _kickApi.CycleTls.SendAsync(
            KickApi.PostSpoofOptions(
                "mobile/login", 
                request, 
                new Dictionary<string, string> {
                    { "Accept", "application/json" },
                    { "Content-Type", "application/json" }
                }
            )
        );
        var token        = JsonSerializer.Deserialize<AuthenticationToken>(loginRequest.Body);

        return token?.Token ?? string.Empty;

    }

    /// <summary>
    /// Sets the global token using the <see cref="Login">Login</see> function
    /// </summary>
    /// <remarks>2 Factor Authentication is not yet supported by the library, but it is by the API</remarks>
    /// <param name="password">User's password</param>
    /// <param name="username">(optional) The username of the user</param>
    /// <param name="email">(optional) The email of the user</param>
    /// <remarks>Username and/or Email must be passed</remarks>
    public async Task SelfAuthenticate(string password, string username = "", string email = "") =>
        _kickApi.Token = await Login(password, username, email);

}