using FluentValidation;
using Recetron.Core.Models;

namespace Recetron.Api.Validators
{
  public class SignupValidator : AbstractValidator<SignUpPayload>
  {
    public SignupValidator()
    {
      RuleFor(s => s.Name).NotNull().NotEmpty();
      RuleFor(s => s.LastName).NotNull().NotEmpty();
      RuleFor(s => s.Email).NotNull().NotEmpty().EmailAddress();
      RuleFor(s => s.Password).NotNull().NotEmpty().Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,30}$");
    }
  }
}
