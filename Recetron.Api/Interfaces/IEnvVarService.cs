using System.Threading.Tasks;
using MongoDB.Driver;
using Recetron.Core.Models;
using Recetron.Api.Models;
using System.Threading;

namespace Recetron.Api.Interfaces
{
  public interface IEnvVarService
  {
    string GetJwtSecret();
    string GetUnsplashAccessToken();
  }
}