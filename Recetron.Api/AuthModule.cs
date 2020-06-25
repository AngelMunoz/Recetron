using Carter;
using Carter.ModelBinding;
using Carter.Response;
using Microsoft.AspNetCore.Http;
using Recetron.Api.Interfaces;
using Recetron.Core.Models;

namespace Recetron.Api
{

  public class AuthModule : CarterModule
  {
    public AuthModule(IAuthService _auth) : base("/auth/")
    {
      Post("login", async (req, res) =>
      {
        var payload = await req.Bind<LoginPayload>();


        var (canLogin, user) = await _auth.VerifyUserLoginAsync(payload);
        if (canLogin)
        {
          var token = _auth.SignJwtToken(user!);
          await res.Negotiate(new AuthResponse() { Token = token, User = user });
          return;
        }
        res.StatusCode = 400;
        await res.Negotiate(new ErrorResponse { Message = "Credentials Not Valid" });
      });

      Post("signup", async (req, res) =>
      {
        var payload = await req.Bind<SignUpPayload>();
        try
        {
          var user = await _auth.SignupUserAsync(payload);
          if (user == null)
          {
            res.StatusCode = 500;
            await res.Negotiate(new ErrorResponse { Message = "Failed to signup user" });
            return;
          }
          var token = _auth.SignJwtToken(user);
          await res.Negotiate(new AuthResponse() { Token = token, User = user });
        }
        catch (System.ArgumentException e)
        {
          res.StatusCode = 400;
          await res.Negotiate(new ErrorResponse { Message = e.Message });
        }

      });
    }
  }
}
