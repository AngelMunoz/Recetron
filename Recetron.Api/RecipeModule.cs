using System;
using System.Linq;
using System.Threading.Tasks;
using Carter;
using Carter.ModelBinding;
using Carter.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using MongoDB.Bson;
using Recetron.Api.Interfaces;
using Recetron.Core.Models;

namespace Recetron.Api
{
  public class RecipeModule : CarterModule
  {
    public RecipeModule(IAuthService _auth, IRecipeService _recipes) : base("/api/recipes")
    {
      this.Before = ctx => ModuleHookHelpers.VerifyJwt(ctx, _auth);
      Get("", async (req, res) =>
      {
        var user = await _auth.ExtractUserAsync(ModuleHookHelpers.ExtractTokenStr(req.HttpContext));
        if (user == null)
        {
          res.StatusCode = 422;
          await res.Negotiate(new ErrorResponse { Message = "Missing User from Token" });
          return;
        }
        var _page = req.Query.FirstOrDefault(f => f.Key == "page").Value.FirstOrDefault();
        var _limit = req.Query.FirstOrDefault(f => f.Key == "limit").Value.FirstOrDefault();
        var (page, limit) = ModuleHookHelpers.GetPagination(_page, _limit);
        var recipes = await _recipes.Find(page, limit, recipe => recipe.UserId == ObjectId.Parse(user.Id));
        await res.Negotiate(recipes);
        return;
      });
    }
  }
}
