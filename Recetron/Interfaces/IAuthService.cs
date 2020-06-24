using System;
using System.Threading.Tasks;
using Recetron.Core.Models;

namespace Recetron.Interfaces
{
  public interface IAuthService
  {
    bool IsAuthenticated { get; }
    string Token { get; }

    Task<bool> LoginAsync(LoginPayload payload);

    Task<bool> SignupAsync(SignUpPayload payload);
    Task LogoutAsync();

    /// When the Authentication is true the user logged in otherwise the user logged out
    event EventHandler<bool> AuthStateChanged;

  }
}