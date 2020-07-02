using Recetron.Core.Interfaces;

namespace Recetron.Api
{
  using System;
  using System.Diagnostics;
  using System.Security.Claims;
  using System.Text;
  using Carter;
  using Microsoft.AspNetCore.Authentication.JwtBearer;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Microsoft.IdentityModel.Tokens;
  using Recetron.Api.Interfaces;
  using Recetron.Api.Services;

  public class Startup
  {
    private readonly IConfiguration Configuration;
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors();
      ConfigureJWT(services);
      services
        .AddScoped<IEnvVarService, EnvVarService>()
        .AddSingleton<IDBService, DBService>()
        .AddScoped<IBackgroundPictureService, BackgroundPictureService>()
        .AddScoped<IAuthService, AuthService>()
        .AddScoped<IRecipeService, RecipeService>();

      services
        .AddCarter();
    }

    private void ConfigureJWT(IServiceCollection services)
    {
      var jwtkey = Environment.GetEnvironmentVariable("JWT_KEY") ?? "Some super secret key";
      var key = Encoding.UTF8.GetBytes(jwtkey);
      services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(jwt =>
        {
          jwt.RequireHttpsMetadata = false;
          jwt.SaveToken = true;
          jwt.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
          };
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
      }

      app.UseRouting();
      app.UseAuthentication();
      app.UseEndpoints(builder => builder.MapCarter());

    }
  }
}
