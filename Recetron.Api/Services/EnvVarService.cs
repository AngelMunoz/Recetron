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
  }
}