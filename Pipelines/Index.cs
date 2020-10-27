using Jamstack.On.Dotnet.Models;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Urls.QueryParameters.Filters;
using Kontent.Statiq;
using Statiq.Common;
using Statiq.Core;
using Statiq.Razor;
using System;
using System.Collections.Generic;
using System.Text;

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
                    .WithQuery(new EqualsFilter("system.codename", "home_page")), 
                // Set the output path for each article
                new SetDestination(Config.FromDocument((doc, ctx)
                  => new NormalizedPath($"index.html"))),
            };

            ProcessModules = new ModuleList
            {
                new MergeContent(new ReadFiles("Index.cshtml")),
                new RenderRazor()
                    .WithModel(Config.FromDocument((document) => document.AsKontent<LandingPage>()))
            };

            OutputModules = new ModuleList {
                new WriteFiles()
            };
        }
    }
}
