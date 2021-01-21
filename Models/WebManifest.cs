using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Jamstack.On.Dotnet.Models
{

    public class WebManifest
    {
        public static IEnumerable<int> IconDimensions = new int[]
        {
            48,
            72,
            96,
            144,
            192,
            256,
            384,
            512,
        };

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icons")]
        public IEnumerable<WebManifestIcon> Icons { get; set; }

        [JsonPropertyName("start_url")]
        public string StartUrl { get; set; }

        [JsonPropertyName("background_color")]
        public string BackgroundColor { get; set; }

        [JsonPropertyName("display")]
        public string Display { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("theme_color")]
        public string ThemeColor { get; set; }

        [JsonPropertyName("shortcuts")]
        public IEnumerable<WebManifestShortcut> Shortcuts { get; set; }

        public WebManifest(Root rootMetadata)
        {
            ShortName = rootMetadata.ShortName;
            Name = rootMetadata.Title;
            Description = rootMetadata.Description;

            var iconData = rootMetadata.Icon.FirstOrDefault();
            Icons = IconDimensions.Select(dimension => new WebManifestIcon
            {
                Src = $"{iconData?.Url}?w={dimension}",
                Type = iconData.Type,
                Sizes = $"{dimension}x{dimension}"
            });

            StartUrl = rootMetadata.StartUrl;
            BackgroundColor = rootMetadata.BackgroundColor;
            Display = rootMetadata.Display;
            Scope = rootMetadata.Scope;
            ThemeColor = rootMetadata.ThemeColor;

            Shortcuts = rootMetadata.ShortcutsTyped.Select(shortcut =>
            new WebManifestShortcut()
            {
                Name = shortcut.Name,
                ShortName = shortcut.Shortname,
                Description = shortcut.Description,
                Url = shortcut.Url,
                Icons = IconDimensions.Select(dimension => new WebManifestIcon
                {
                    Src = $"{shortcut.Icon.FirstOrDefault()?.Url}?w={dimension}",
                    Type = shortcut.Icon.FirstOrDefault().Type,
                    Sizes = $"{dimension}x{dimension}"
                })
            });
        }

    }
}
