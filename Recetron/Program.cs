using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Blazored.LocalStorage;
using Recetron.Core.Interfaces;
using Recetron.Interfaces;
using Recetron.Services;

namespace Recetron
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            ConfigureServices(builder.Services, builder.Configuration);

            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, WebAssemblyHostConfiguration config)
        {
            var baseApiUrl = config.GetSection("apiUrl")?.Value;
            services
              .AddBlazoredLocalStorage()
              .AddScoped<IInteropService, InteropService>()
              .AddScoped<IAuthService, AuthService>()
              .AddScoped<IRecipeService, RecipeService>()
              .AddScoped<IToastService, ToastService>();

            services.AddHttpClient(Constants.AUTH_CLIENT_NAME, client => client.BaseAddress = new Uri($"{baseApiUrl}/auth"));
            services.AddHttpClient(Constants.BG_PICTURE_URL_NAME, client => client.BaseAddress = new Uri($"{baseApiUrl}/api/background-picture"));
            services.AddHttpClient(Constants.API_CLIENT_NAME, (sp, client) =>
              {
                  client.BaseAddress = new Uri($"{baseApiUrl}/api");
                  var sfactory = sp.GetService<IServiceScopeFactory>();
                  using var scope = sfactory.CreateScope();
                  var auth = scope.ServiceProvider.GetService<IAuthService>();
                  var token = auth?.Token;
                  if (token == null) { return; }
                  client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
              });
        }
    }

}
