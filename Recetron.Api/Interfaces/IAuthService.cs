using System.Threading.Tasks;
using MongoDB.Driver;
using Recetron.Core.Models;
using Recetron.Api.Models;
using System.Threading;

namespace Recetron.Api.Interfaces
{
  public interface IAuthService
  {
    bool VerifyJwt(string token);
    string SignJwtToken(UserDTO user);

    Task<UserDTO?> ExtractUserAsync(string? token);
    Task<UserDTO?> SignupUserAsync(SignUpPayload payload, CancellationToken ct = default);
    Task<(bool, UserDTO?)> VerifyUserLoginAsync(LoginPayload payload, CancellationToken ct = default);

  }
}