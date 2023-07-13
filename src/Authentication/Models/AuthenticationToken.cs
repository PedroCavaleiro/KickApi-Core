using System.Text.Json.Serialization;

namespace KickStreaming.Authentication.Models; 

public class AuthenticationToken {
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("2fa_required")]
    public bool _2faRequired { get; set; }
}