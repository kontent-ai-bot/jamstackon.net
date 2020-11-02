using Jamstack.On.Dotnet.Models;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Urls.QueryParameters.Filters;
using Kontent.Statiq;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;


namespace Jamstack.On.Dotnet.Pipelines
{
    public class Index : Pipeline
    {
        public Index(IDeliveryClient client)
        {
            InputModules = new ModuleList
            {
                // Load home page
                new Kontent<LandingPage>(client)
                    .WithQuery(new EqualsFilter("system.codename", "home_page"))
            };

            ProcessModules = new ModuleList
            {
                new MergeContent(new ReadFiles("Index.cshtml")),
                new RenderRazor()
                    .WithModel(Config.FromDocument((document, context) =>
                        document.AsKontent<LandingPage>())),
                new SetDestination(Config.FromDocument((doc, ctx)
                  => new NormalizedPath($"index.html")))
            };

            OutputModules = new ModuleList {
                new WriteFiles()
            };
        }
    }
}
