using System;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Recetron.Core.Interfaces;
using Recetron.Interfaces;
using Recetron.Services;
using Recetron;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("app");
ConfigureServices(builder.Services, builder.Configuration);
await builder.Build().RunAsync();

static void ConfigureServices(IServiceCollection services, IConfiguration config)
{
  var baseApiUrl = config.GetSection("apiUrl")?.Value;
  services
    .AddBlazoredLocalStorage()
    .AddScoped<IAuthService, AuthService>()
    .AddScoped<IRecipeService, RecipeService>()
    .AddScoped<IToastService, ToastService>();

  services.AddHttpClient(Constants.AUTH_CLIENT_NAME, client => client.BaseAddress = new Uri($"{baseApiUrl}/auth"));
  services.AddHttpClient(Constants.BG_PICTURE_URL_NAME, client => client.BaseAddress = new Uri($"{baseApiUrl}/api/background-picture"));
  services.AddHttpClient(Constants.API_CLIENT_NAME, (sp, client) =>
    {
      client.BaseAddress = new Uri($"{baseApiUrl}/api");
      var serviceFactory = sp.GetService<IServiceScopeFactory>();
      using var scope = serviceFactory?.CreateScope();
      var auth = scope?.ServiceProvider.GetService<IAuthService>();
      var token = auth?.Token;
      if (token == null) { return; }
      client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    });
}