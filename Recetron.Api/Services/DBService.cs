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
      var connstring = Environment.GetEnvironmentVariable("MONGO_URL") ?? "mongodb://localhost:27017/recetron";
      _client = new MongoClient(connstring);
      _db = _client.GetDatabase("recetron");
    }

    public IMongoCollection<T> GetCollection<T>(string name) => _db.GetCollection<T>(name);

  }
}