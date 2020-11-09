using Jamstack.On.Dotnet.Models;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Statiq.App;
using Statiq.Common;
using Statiq.Web;
using System;
using System.Threading.Tasks;

namespace Jamstack.On.Dotnet
{
    class Program
    {
        public static async Task<int> Main(string[] args) =>
          await Bootstrapper
            .Factory
            .CreateWeb(args)
            // Preview API key + turn on Preview API
            .BuildConfiguration(cfg => cfg.AddUserSecrets<Program>())
            .ConfigureServices((services, settings) =>
            {
                services.AddSingleton<ITypeProvider, CustomTypeProvider>();
                services.AddDeliveryClient((IConfiguration)settings);
            })
            .RunAsync();
    }
}
