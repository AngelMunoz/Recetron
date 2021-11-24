using System.Threading.Tasks;
using Carter;
using Carter.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Recetron.Api.Interfaces;

namespace Recetron.Api
{
  public class PictureModule : ICarterModule
  {
    private async Task<IResult> OnGetBgPicture(IBackgroundPictureService pictureService)
    {
      var picture = await pictureService.GetDailyPicture();
      return Results.Ok(picture);
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
      app.MapGet("/api/background-picture", OnGetBgPicture).AllowAnonymous();
    }
  }
}