using System.Text.Json.Serialization;

namespace WebToApp2.Models.Sima
{
    public class Header
    {
        [JsonPropertyName("AlgName")] public string AlgorithmName { get; set; }

        [JsonPropertyName("Signature")] public byte[] Signature { get; set; }
    }
}
