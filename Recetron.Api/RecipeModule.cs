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
      this.Before = ctx => ModuleHelpers.VerifyJwt(ctx, _auth);
      Get("", async (req, res) =>
      {
        var user = await _auth.ExtractUserAsync(ModuleHelpers.ExtractTokenStr(req.HttpContext));
        if (user == null)
        {
          res.StatusCode = 422;
          await res.Negotiate(new ErrorResponse { Message = "Missing User from Token" });
          return;
        }
        var _page = req.Query.FirstOrDefault(f => f.Key == "page").Value.FirstOrDefault();
        var _limit = req.Query.FirstOrDefault(f => f.Key == "limit").Value.FirstOrDefault();
        var (page, limit) = ModuleHelpers.GetPagination(_page, _limit);
        var recipes = await _recipes.FindByUser(user.Id ?? ObjectId.Empty, page, limit);
        await res.Negotiate(recipes);
        return;
      });

      Post("", async (req, res) =>
      {
        var user = await _auth.ExtractUserAsync(ModuleHelpers.ExtractTokenStr(req.HttpContext));
        if (user?.Id == null)
        {
          res.StatusCode = 422;
          await res.Negotiate(new ErrorResponse { Message = "Missing User from Token" });
          return;
        }

        var result =  await req.BindAndValidate<Recipe>();
        if (!result.ValidationResult.IsValid)
        {
          res.StatusCode = 400;
          await res.Negotiate(
            new ErrorResponse
            {
              Message = "Failed Validation",
              Errors = result.ValidationResult.GetFormattedErrors()
            }
          );
          return;
        }

        var payload = result.Data;

        payload.UserId = user.Id;
        var recipe = await _recipes.Create(payload);

        res.StatusCode = 201;
        await res.Negotiate(recipe);
      });
    }
  }
}
