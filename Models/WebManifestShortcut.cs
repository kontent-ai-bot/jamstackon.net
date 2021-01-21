using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Jamstack.On.Dotnet.Models
{
    public class WebManifestShortcut
    {

        [JsonPropertyName("Name")]
        public string Name{ get; set; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("icons")]
        public IEnumerable<WebManifestIcon> Icons { get; set; }
    }
}
