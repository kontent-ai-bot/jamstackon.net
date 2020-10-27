using Jamstack.On.Dotnet.Models;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Extensions;
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
            .ConfigureServices((services, settings) =>
            {
                services.AddSingleton<ITypeProvider, CustomTypeProvider>();
                services.AddDeliveryClient(options =>
                    options
                        .WithProjectId("1981ac13-ec8e-00fd-273b-d8cfd86ed5ba")
                        .UseProductionApi()
                        .Build()
                    );
            })
            .RunAsync();
    }
}
