using System.ComponentModel.DataAnnotations;

namespace Recetron.Core.Models
{
  public class SignUpPayload
  {
    [Required, MinLength(4)]
    public string? Email { get; set; }

    [Required, RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,30}$", ErrorMessage = "Password must contain at least 8 characters, a lowercase letter and an uppercase letter")]
    public string? Password { get; set; }
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? LastName { get; set; }
  }
}