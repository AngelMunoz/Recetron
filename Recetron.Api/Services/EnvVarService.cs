using System;
using Recetron.Api.Interfaces;

namespace Recetron.Api.Services
{
  public class EnvVarService : IEnvVarService
  {
    public string GetJwtSecret()
    {
      return Environment.GetEnvironmentVariable("JWT_KEY") ?? "Some super secret key";
    }

    public string GetUnsplashAccessToken()
    {
      return Environment.GetEnvironmentVariable("UNSPLASH_API") ?? throw new NullReferenceException("Unsplash Access Token Not found in Environment");
    }
  }
}