using System.Threading.Tasks;
using Recetron.Core.Models;
using System.Threading;
using System.Collections.Generic;
using Recetron.Core.Interfaces;

namespace Recetron.Api.Interfaces
{
  public interface IRecipeService : ICrudable<Recipe>
  {

    Task<IEnumerable<Recipe>> FindByNameAsync(string recipeName, CancellationToken ct = default);
  }
}