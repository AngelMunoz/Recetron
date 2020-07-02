using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Http;
using System.Text;
using Recetron.Api.Interfaces;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Recetron.Core.Models;
using MongoDB.Driver;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Recetron.Api.Services
{
  public class BackgroundPictureService : IBackgroundPictureService
  {
    private readonly IMongoCollection<DailyPictureRecord> pictures;
    private readonly IEnvVarService env;

    public BackgroundPictureService(IDBService db, IEnvVarService env)
    {
      pictures = db.GetCollection<DailyPictureRecord>("dailypicture");
      this.env = env;
    }


    public async Task<UnsplashPicture> GetDailyPicture(CancellationToken ct = default)
    {
      async Task<UnsplashPicture> PersistRecord()
      {
        var newRecord = await RequestNewPicture();
        await pictures.InsertOneAsync(newRecord);
        return newRecord.Picture!;
      }

      var record = await pictures
        .Find(FilterDefinition<DailyPictureRecord>.Empty)
        .SortByDescending(r => r.CreatedAt)
        .FirstOrDefaultAsync(cancellationToken: ct);

      if (record == null) { return await PersistRecord(); }

      var difference = DateTimeOffset.Now.Subtract(record.CreatedAt);

      if (difference.TotalDays < 1) { return record.Picture!; }
      else { return await PersistRecord(); }
    }

    private async Task<DailyPictureRecord> RequestNewPicture()
    {
      using var http = new HttpClient();
      var token = env.GetUnsplashAccessToken();
      var uri = new Uri($"https://api.unsplash.com/photos/random?client_id={token}&query=Food&orientation=landscape");
      var request = await http.GetAsync(uri);
      var jsonstr = await request.Content.ReadAsStringAsync();
      var picture = JsonSerializer.Deserialize<UnsplashPicture>(jsonstr);
      return new DailyPictureRecord() { CreatedAt = DateTimeOffset.Now, Picture = picture };
    }
  }
}
