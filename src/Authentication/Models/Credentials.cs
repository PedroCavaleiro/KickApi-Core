using System.Text.Json.Serialization;

namespace KickStreaming.Authentication.Models; 

public class Credentials {
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("one_time_password")]
    public bool OneTimePassword { get; set; }
}