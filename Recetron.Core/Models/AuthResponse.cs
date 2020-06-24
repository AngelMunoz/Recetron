namespace Recetron.Core.Models
{
  public class AuthResponse
  {
    public string? Token { get; set; }
    public UserDTO? User { get; set; }
  }
}