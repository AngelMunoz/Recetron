using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Recetron.Api.Interfaces;
using Recetron.Core.Interfaces;
using Recetron.Core.Models;
using Carter;
using Carter.ModelBinding;
using Microsoft.AspNetCore.Authorization;

namespace Recetron.Api
{
  public class RecipeModule : ICarterModule
  {
    [Authorize]
    private async Task<IResult> OnFindAllRecipes(IRecipeService _recipes, IAuthService _auth, HttpRequest req)
    {
      var user = await _auth.ExtractUserAsync(ModuleHelpers.ExtractTokenStr(req.HttpContext));
      if (user is null)
      {
        return Results.UnprocessableEntity(new ErrorResponse("Missing User from Token"));
      }

      var _page = req?.Query?.FirstOrDefault(f => f.Key == "page").Value.FirstOrDefault();
      var _limit = req?.Query?.FirstOrDefault(f => f.Key == "limit").Value.FirstOrDefault();
      var (page, limit) = ModuleHelpers.GetPagination(_page, _limit);
      var recipes = await _recipes.FindByUser(user.Id ?? string.Empty, page, limit);
      return Results.Ok(recipes);
    }

    [Authorize]
    private async Task<IResult> OnFineOneRecipe(string id, IRecipeService _recipes, IAuthService _auth, HttpContext ctx)
    {
      var recipe = await _recipes.FindOne(id);
      var user = await _auth.ExtractUserAsync(ModuleHelpers.ExtractTokenStr(ctx));
      return recipe?.UserId != user?.Id ? Results.Forbid() : Results.Ok(recipe);
    }

    [Authorize]
    public async Task<IResult> OnCreateRecipe(Recipe payload, IRecipeService _recipes, IAuthService _auth,
      HttpContext ctx)
    {
      var user = await _auth.ExtractUserAsync(ModuleHelpers.ExtractTokenStr(ctx));
      if (user?.Id is null)
      {
        return Results.UnprocessableEntity(new ErrorResponse("Missing User from Token"));
      }

      var result = ctx.Request.Validate(payload);
      if (!result.IsValid)
      {
        return Results.BadRequest(new ErrorResponse("Failed Validation") { Errors = result.GetFormattedErrors() });
      }

      var recipe = await _recipes.Create(payload with { UserId = user.Id });

      return Results.Created($"/api/recipes/{recipe.Id}", recipe);
    }

    [Authorize]
    private async Task<IResult> OnEditRecipe(Recipe recipe, IAuthService _auth, IRecipeService _recipes,
      HttpContext ctx)
    {
      var user = await _auth.ExtractUserAsync(ModuleHelpers.ExtractTokenStr(ctx));
      if (user?.Id is null)
      {
        return Results.UnprocessableEntity(new ErrorResponse("Missing User from Token"));
      }

      var validationResult = ctx.Request.Validate(recipe);
      if (!validationResult.IsValid)
      {
        return Results.BadRequest(new ErrorResponse("Failed Validation")
        {
          Errors = validationResult.GetFormattedErrors()
        });
      }

      if (recipe.UserId != user.Id)
      {
        return Results.Forbid();
      }

      var didUpdate = await _recipes.Update(recipe);
      return Results.Ok(didUpdate);
    }

    [Authorize]
    public async Task<IResult> OnDeleteRecipe(string id, IAuthService _auth, IRecipeService _recipes, HttpContext ctx)
    {
      var recipe = await _recipes.FindOne(id);
      var user = await _auth.ExtractUserAsync(ModuleHelpers.ExtractTokenStr(ctx));
      if (recipe.UserId != user?.Id)
      {
        return Results.Forbid();
      }

      var didDestroy = await _recipes.Destroy(id);
      return didDestroy
        ? Results.NoContent()
        : Results.UnprocessableEntity(new ErrorResponse("Failed To Delete This Recipe"));
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
      app.MapGet("/api/recipes", OnFindAllRecipes);
      app.MapGet("/api/recipes/{id}", OnFineOneRecipe);
      app.MapPost("/api/recipes", OnCreateRecipe);
      app.MapPut("/api/recipes", OnEditRecipe);
      app.MapDelete("/api/recipes/{id}", OnDeleteRecipe);
    }
  }
}