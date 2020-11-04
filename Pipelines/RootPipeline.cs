using Jamstack.On.Dotnet.Models;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Urls.QueryParameters;
using Kentico.Kontent.Delivery.Urls.QueryParameters.Filters;
using Kontent.Statiq;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
                    new ExecuteConfig(
                        Config.FromDocument((doc, context) => 
                            doc.AsKontent<Root>().Subpages.ToList().Select(subpage =>
                            {
                                var pageContent = (subpage as Page)?.Content.FirstOrDefault();
                                if(pageContent == null)
                                {
                                        throw new InvalidDataException("Root page (codename: root, type: root) does not contain any pages, or any page does not contain exactly content block!");
                                }

                                return context.CreateDocument(
                                    CreateKontentDocument(context, pageContent));
                            })
                        )
                    )
                )
            };

            ProcessModules = new ModuleList {
                new MergeContent(new ReadFiles("LandingPage.cshtml")),
                new RenderRazor()
                    .WithModel(Config.FromDocument((document, context) =>
                    {
                        var typeCodename = document
                            .FilterMetadata(KontentKeys.System.Type)
                            .Values
                            ?.FirstOrDefault()
                            ?.ToString();
                        Type type = typeProvider.GetType(typeCodename);

                        var landingPageType = typeof(LandingPage);
                        if(landingPageType == type)
                        {
                                return document.AsKontent<LandingPage>();
                        }
                        else
                        {
                                throw new InvalidDataException("Unsuported content type of the Page's Content element");
                        }
                    })),
                new SetDestination(Config.FromDocument((doc, ctx) =>
                  new NormalizedPath("index.html")))
            };

            OutputModules = new ModuleList {
                new WriteFiles()
            };
        }


        private IDocument CreateKontentDocument(IExecutionContext context, object item)
        {
            var props = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy |
                                                            BindingFlags.GetProperty | BindingFlags.Public);
            var metadata = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(TypedContentExtensions.KontentItemKey, item),
            };

            if (props.FirstOrDefault(prop => typeof(IContentItemSystemAttributes).IsAssignableFrom(prop.PropertyType))
                ?.GetValue(item) is IContentItemSystemAttributes systemProp)
            {
                metadata.AddRange(new[]
                {
                    new KeyValuePair<string, object>(KontentKeys.System.Name, systemProp.Name),
                    new KeyValuePair<string, object>(KontentKeys.System.CodeName, systemProp.Codename),
                    new KeyValuePair<string, object>(KontentKeys.System.Language, systemProp.Language),
                    new KeyValuePair<string, object>(KontentKeys.System.Id, systemProp.Id),
                    new KeyValuePair<string, object>(KontentKeys.System.Type, systemProp.Type),
                    new KeyValuePair<string, object>(KontentKeys.System.LastModified, systemProp.LastModified)
                });
            }

            return context.CreateDocument(metadata, null, "text/html");
        }
    }
}
