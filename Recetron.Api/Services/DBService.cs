using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Recetron.Api.Interfaces;

namespace Recetron.Api.Services
{
  public class DBService : IDBService
  {
    private MongoClient _client;
    private IMongoDatabase _db;

    public DBService()
    {
      var connstring = Environment.GetEnvironmentVariable("MONGO_URL") ?? "mongodb://192.168.100.5:27017/recetron";
      var settings = MongoClientSettings.FromConnectionString(connstring);
      _client = new MongoClient(settings);
      _db = _client.GetDatabase("recetron");
    }

    public IMongoCollection<T> GetCollection<T>(string name) => _db.GetCollection<T>(name);

  }
}