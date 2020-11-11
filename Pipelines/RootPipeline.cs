using Jamstack.On.Dotnet.Models;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Urls.QueryParameters;
using Kentico.Kontent.Delivery.Urls.QueryParameters.Filters;
using Kontent.Statiq;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using System;
using System.Linq;

namespace Jamstack.On.Dotnet.Pipelines
{
    public class RootPipeline : Pipeline
    {
        public RootPipeline(IDeliveryClient client, ITypeProvider typeProvider)
        {
            InputModules = new ModuleList {
                new Kontent<Root>(client)
                    .WithQuery(
                        new EqualsFilter("system.codename", "root"),
                        new LimitParameter(1),
                        new DepthParameter(3)
                    ),
                new ReplaceDocuments(
                    KontentConfig.GetChildren<Root>( page =>
                        page.Subpages.OfType<Page>().SelectMany(item => item.Content)
                        )
                    ),
            };

            ProcessModules = new ModuleList {
                new MergeContent(
                    new ReadFiles(
                        Config.FromDocument((document, context) => {
                            var typeCodename = document
                                .FilterMetadata(KontentKeys.System.Type)
                                .Values
                                ?.FirstOrDefault()
                                ?.ToString();

                            switch(typeCodename)
                            {
                                case LandingPage.Codename:
                                    return "LandingPage.cshtml";
                                default:
                                    throw new NotImplementedException($"Template not implemented for page content type {typeCodename}");
                            }
                        })
                    )
                ),
                new RenderRazor()
                    .WithModel(Config.FromDocument((document, context) =>
                    {
                        var typeCodename = document
                            .FilterMetadata(KontentKeys.System.Type)
                            .Values
                            ?.FirstOrDefault()
                            ?.ToString();

                        switch (typeCodename)
                        {
                            case LandingPage.Codename:
                                return document.AsKontent<LandingPage>();
                            default:
                                throw new NotImplementedException($"Template not implemented for page content type {typeCodename}");
                        }
                    })),
                new SetDestination(Config.FromDocument((doc, ctx) =>
                  new NormalizedPath("index.html")))
            };

            OutputModules = new ModuleList {
                new WriteFiles()
            };
        }
    }
}
