using System;
using System.Linq;
using System.Collections.Generic;
using Kentico.Kontent.Delivery.Abstractions;

namespace Jamstack.On.Dotnet.Models
{
    public partial class LandingPage
    {
        public IEnumerable<Link> HeaderLinksTyped => HeaderLinks.OfType<IInlineContentItem>().Select(i => i.ContentItem).Cast<Link>();

        public bool ShowDarkModeToggle => Options.Any(option => option.Codename == "dark_mode_switch");
    }
}