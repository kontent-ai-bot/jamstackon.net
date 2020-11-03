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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/*
Subbmit here: https://github.com/alanta/Kontent.Statiq/issues/new?assignees=&labels=&template=feature_request.md&title=
 
 **Is your feature request related to a problem? Please describe.**
If you want to load Linked Items of the particular item, it is required to transform them to `IDocument` to be able to pass them through the pipeline.

i.e. You have a "Root" item that is the root of your website, this contains the Linked Items element (`Subpages`) model menu structure. Every item (`Page` type) then contains another `Subpages` element to allow having a multilevel navigation menu. Plus the `Page` model contains "Content" Linked items element containing typically one item holding the channel-agnostic content.

> **Basicall [Webspotlight](https://docs.kontent.ai/tutorials/set-up-kontent/set-up-your-project/web-spotlight) setup.**

Example model structure:

```csharp   
public partial class Root
{
        public IEnumerable<object> Subpages { get; set; } // Strongly types items of type i.e. Page
}

public partial class Page
{
        public IEnumerable<object> Subpages { get; set; } // Strongly types items of type i.e. Page
        public IEnumerable<object> Content { get; set; } // Contains only one "Content" item
}
```

Then in your pipeline, you could use: 

**Describe the solution you'd like**
A clear and concise description of what you want to happen.

**Describe alternatives you've considered**
A clear and concise description of any alternative solutions or features you've considered.

**Additional context**
Add any other context or screenshots about the feature request here.

 */

namespace Jamstack.On.Dotnet.Pipelines
{
    public class RootPipeline : Pipeline
    {
        public RootPipeline(IDeliveryClient client)
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
                        Config.FromDocument((doc, context) => {
                            IDocument result = null;

                            doc.AsKontent<Root>().Subpages.ToList().ForEach(subpage =>
                            {
                                switch (subpage)
                                {
                                    case Page page:
                                        switch (page.Content.FirstOrDefault())
                                        {
                                            case LandingPage landingPage:
                                               result = context.CreateDocument(CreateKontentDocument(context, landingPage));
                                                return;
                                            default:
                                                break;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            });
                            return result;
                        })
                    )
                )
            };

            ProcessModules = new ModuleList {
                new MergeContent(new ReadFiles("LandingPage.cshtml")),
                new RenderRazor()
                    .WithModel(Config.FromDocument((document, context) =>
                    {

                        return document.AsKontent<LandingPage>();
                    })),
                new SetDestination(Config.FromDocument((doc, ctx) =>
                  new NormalizedPath($"{doc.AsKontent<LandingPage>().System.Codename}.html")))
            };

            OutputModules = new ModuleList {
                new WriteFiles()
            };
        }


        private IDocument CreateKontentDocument(IExecutionContext context, LandingPage item)
        {
            var props = typeof(LandingPage).GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy |
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
