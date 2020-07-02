using Carter;
using Carter.Response;
using Recetron.Api.Interfaces;

namespace Recetron.Api
{
  public class PictureModule : CarterModule
  {
    public PictureModule(IBackgroundPictureService pictureService)
      : base("/api/background-picture")
    {
      Get("", async (req, res) =>
      {
        var picture = await pictureService.GetDailyPicture();
        await res.Negotiate(picture);
        return;
      });
    }
  }
}
