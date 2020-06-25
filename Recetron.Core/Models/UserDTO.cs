using MongoDB.Bson;

namespace Recetron.Core.Models
{
  public class UserDTO
  {
    public ObjectId? Id { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
  }
}