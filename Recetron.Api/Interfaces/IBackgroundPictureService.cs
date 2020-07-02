using Recetron.Core.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Recetron.Api.Interfaces
{
  public interface IBackgroundPictureService
  {
    Task<UnsplashPicture> GetDailyPicture(CancellationToken ct = default);
  }
}
