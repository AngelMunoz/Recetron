using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Blazored.LocalStorage;
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

    public static void ConfigureServices(IServiceCollection services, WebAssemblyHostConfiguration config)
    {
      var baseApiUrl = config.GetSection("apiUrl")?.Value;
      services
        .AddBlazoredLocalStorage()
        .AddScoped<IAuthService, AuthService>()
        .AddHttpClient(Constants.AUTH_CLIENT_NAME, client => client.BaseAddress = new Uri($"{baseApiUrl}/auth"));

      services.AddHttpClient(Constants.API_CLIENT_NAME, (sp, client) =>
        {
          client.BaseAddress = new Uri($"{baseApiUrl}/api");
          var auth = sp.GetService<IAuthService>();
          var token = auth.Token;
          if (token is null) { return; }
          client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        });
    }
  }

}
