using System;
using System.Threading.Tasks;
using Carter;
using Carter.ModelBinding;
using Carter.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Recetron.Api.Interfaces;
using Recetron.Core.Models;

namespace Recetron.Api
{
  public class AuthModule : ICarterModule
  {
    private async Task<IResult> OnLogin(LoginPayload login, IAuthService auth)
    {
      var (canLogin, user) = await auth.VerifyUserLoginAsync(login);
      if (!canLogin || user is null)
        return Results.BadRequest(new ErrorResponse("Credentials Not Valid"));

      var token = auth.SignJwtToken(user);
      return Results.Ok(new AuthResponse(token, user));
    }

    private async Task<IResult> OnSignUp(SignUpPayload payload, IAuthService _auth, HttpContext ctx)
    {
      var result = ctx.Request.Validate(payload);
      if (!result.IsValid)
      {
        return Results.BadRequest(new ErrorResponse("Failed Validation") { Errors = result.GetFormattedErrors() });
      }

      try
      {
        var user = await _auth.SignupUserAsync(payload);
        if (user is null)
        {
          return Results.UnprocessableEntity(new ErrorResponse("Failed to signup user"));
        }

        var token = _auth.SignJwtToken(user);
        return Results.Ok(new AuthResponse(token, user));
      }
      catch (System.ArgumentException e)
      {
        return Results.BadRequest(new ErrorResponse(e.Message));
      }
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
      app.MapPost("/auth/login", OnLogin).AllowAnonymous();
      app.MapPost("/auth/signup", OnSignUp).AllowAnonymous();
    }
  }
}