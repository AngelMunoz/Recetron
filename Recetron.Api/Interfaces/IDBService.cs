using MongoDB.Driver;

namespace Recetron.Api.Interfaces
{
  public interface IDBService
  {
    IMongoCollection<T> GetCollection<T>(string name);
  }
}