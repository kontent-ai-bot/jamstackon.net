using Jamstack.On.Dotnet.Models;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Urls.QueryParameters;
using Kentico.Kontent.Delivery.Urls.QueryParameters.Filters;
using Kontent.Statiq;
using Statiq.Common;
using Statiq.Core;
using System.Text.Json;

namespace Jamstack.On.Dotnet.Pipelines
{
    public class ManifestPipeline : Pipeline
    {
        public ManifestPipeline(IDeliveryClient client)
        {
            InputModules = new ModuleList
            {
                new Kontent<Root>(client)
                    .WithQuery(
                        new EqualsFilter("system.codename", "root"),
                        new LimitParameter(1),
                        new DepthParameter(1)
                    )
                    .WithContent(data => {
                        var manifest = new WebManifest(data);
                        return JsonSerializer.Serialize(manifest);
                    }),
                 new SetDestination(
                    new NormalizedPath("manifest.webmanifest")
                    ),
            };


            OutputModules = new ModuleList {
                new WriteFiles()
            };
        }
    }
}
