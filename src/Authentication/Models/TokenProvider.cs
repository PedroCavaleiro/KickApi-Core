using System.Text.Json.Serialization;

namespace KickStreaming.Authentication.Models; 

public class TokenProvider {
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("nameFieldName")]
    public string NameFieldName { get; set; }

    [JsonPropertyName("unrandomizedNameFieldName")]
    public string UnrandomizedNameFieldName { get; set; }

    [JsonPropertyName("validFromFieldName")]
    public string ValidFromFieldName { get; set; }

    [JsonPropertyName("encryptedValidFrom")]
    public string EncryptedValidFrom { get; set; }
}