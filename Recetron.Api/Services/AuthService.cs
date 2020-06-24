using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Recetron.Api.Interfaces;
using Recetron.Api.Models;
using Recetron.Core.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Recetron.Api.Services
{
  public class AuthService : IAuthService
  {
    private readonly IMongoCollection<User> _users;
    private readonly IEnvVarService _envvars;
    public AuthService(IEnvVarService envvars, IDBService dbs)
    {
      _users = dbs.GetCollection<User>("users");
      _envvars = envvars;
    }

    public async Task<UserDTO> SignupUserAsync(SignUpPayload payload, CancellationToken ct = default)
    {
      var count = await _users.CountDocumentsAsync(user => user.Email!.Equals(payload.Email), null, ct);
      if (count > 0) { throw new ArgumentException("User Already Exists"); }
      var password = BCryptNet.EnhancedHashPassword(payload.Password);
      var user = new User
      {
        Email = payload.Email,
        LastName = payload.LastName,
        Name = payload.Name,
        Password = password
      };
      await _users.InsertOneAsync(user, null, ct);
      var newUser = await _users.FindAsync(user => user.Email == payload.Email, null, ct);
      var firstuser = newUser.FirstOrDefault(ct);
      return new UserDTO
      {
        Email = firstuser.Email,
        LastName = firstuser.LastName,
        Name = firstuser.Name,
        Id = firstuser.Id.ToString()
      };
    }


    public bool VerifyJWT(string token)
    {

      var tokenhandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_envvars.GetJwtSecret());
      var parameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
      };
      try
      {
        tokenhandler.ValidateToken(token, parameters, out SecurityToken validated);
        return true;
      }
      catch (ArgumentNullException) { return false; }
      catch (ArgumentException) { return false; }
      catch (SecurityTokenEncryptionFailedException) { return false; }
    }

    public string SignJwtToken(UserDTO user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_envvars.GetJwtSecret());
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(
          new Claim[]
          {
                    new Claim(ClaimTypes.Name, user.Id?.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Name?.ToString())
          }
        ),
        Expires = DateTime.UtcNow.AddMinutes(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    public async Task<(bool, UserDTO?)> VerifyUserLoginAsync(LoginPayload payload, CancellationToken ct = default)
    {
      var result = await _users.FindAsync(user => user.Email!.Equals(payload.Email), null, ct);
      var user = result.FirstOrDefault(ct);
      if (user is null) return (false, null);
      return (
        BCryptNet.EnhancedVerify(payload.Password, user.Password),
        new UserDTO
        {
          Email = user.Email,
          LastName = user.LastName,
          Name = user.Name,
          Id = user.Id.ToString()
        }
      );
    }
  }
}