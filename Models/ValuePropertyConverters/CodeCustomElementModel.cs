using System.Text.Json.Serialization;

namespace Jamstack.On.Dotnet.Models
{
    public class CodeCustomElementModel
    {
        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
