using System.Threading.Tasks;
using Recetron.Core.Models;
using System.Threading;
using System.Collections.Generic;
using Recetron.Core.Interfaces;
using MongoDB.Bson;

namespace Recetron.Core.Interfaces
{
  public interface IRecipeService : ICrudable<Recipe>
  {

    Task<IEnumerable<Recipe>> FindByNameAsync(string recipeName, CancellationToken ct = default);
    Task<PaginationResult<Recipe>> FindByUser(ObjectId userId, int page, int limit, CancellationToken ct = default);
  }
}