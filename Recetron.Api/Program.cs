using System;
using System.Text;
using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Recetron.Api;
using Recetron.Api.Interfaces;
using Recetron.Api.Services;
using Recetron.Core.Interfaces;
using Carter;
using Microsoft.AspNetCore.Authorization;

var host = WebApplication.CreateBuilder(args);
host.Services.AddCors();
ConfigureJwt(host.Services);
host.Services
  .AddScoped<IEnvVarService, EnvVarService>()
  .AddSingleton<IDBService, DBService>()
  .AddScoped<IBackgroundPictureService, BackgroundPictureService>()
  .AddScoped<IAuthService, AuthService>()
  .AddScoped<IRecipeService, RecipeService>()
  .AddCarter();

var app = host.Build();

if (host.Environment.IsDevelopment())
{
  app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}
else
{
  app.UseCors(o => o
    .WithMethods("POST", "PUT", "GET", "DELETE", "HEAD", "OPTIONS")
    .AllowAnyHeader()
    .WithOrigins("https://recetron-client.web.app"));
}

app
  .UseAuthentication()
  .UseAuthorization();
app.MapCarter();
await app.RunAsync();


void ConfigureJwt(IServiceCollection services)
{
  var jwtkey = Environment.GetEnvironmentVariable("JWT_KEY") ?? "Some super secret key";
  var key = Encoding.UTF8.GetBytes(jwtkey);

  services.AddAuthorization();
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