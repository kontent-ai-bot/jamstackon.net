using System.Text.Json.Serialization;

namespace Jamstack.On.Dotnet.Models
{
    public class WebManifestIcon
    {
        [JsonPropertyName("src")]
        public string Src { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("sizes")]
        public string Sizes { get; set; }
    }
}
