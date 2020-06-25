using System;
using System.Linq;
using System.Threading.Tasks;
using Carter;
using Carter.ModelBinding;
using Carter.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Recetron.Api.Interfaces;
using Recetron.Core.Models;

namespace Recetron.Api
{
  public static class ModuleHookHelpers
  {
    public static string? ExtractTokenStr(HttpContext ctx)
    {
      ctx.Request.Headers.TryGetValue("Authorization", out StringValues values);
      return values.FirstOrDefault()?.Split(" ").Skip(1).FirstOrDefault();
    }
    public static Func<HttpContext, IAuthService, Task<bool>> VerifyJwt = (ctx, auth) =>
     {
       var token = ExtractTokenStr(ctx);
       if (token == null)
       {
         ctx.Response.StatusCode = 401;
         return Task.FromResult(false);
       }

       var isAllowed = auth.VerifyJWT(token);
       if (!isAllowed)
       {
         ctx.Response.StatusCode = 401;
       }
       return Task.FromResult(isAllowed);
     };

    public static (int page, int limit) GetPagination(string? page, string? limit)
    {
      var p = int.Parse(page ?? "0");
      var l = int.Parse(limit ?? "10");
      return (p, l);
    }
  }
}
