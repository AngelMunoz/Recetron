using FluentValidation;
using Recetron.Core.Models;

namespace Recetron.Api.Validators
{
  public class RecipeValidator : AbstractValidator<Recipe>
  {
    public RecipeValidator()
    {
      RuleFor(r => r.Title)
        .NotNull()
        .NotEmpty()
        .MinimumLength(4);

      RuleFor(r => r.Description)
        .NotEmpty()
        .MaximumLength(500)
        .When(r => r.Description != null);

      RuleFor(r => r.ImageUrl)
        .NotEmpty()
        .When(r => r.ImageUrl != null);

      RuleFor(r => r.Notes)
        .NotEmpty()
        .MaximumLength(240)
        .When(r => r.Notes != null);

      RuleForEach(r => r.Steps).SetValidator(new RecipeStepValidator());
      RuleForEach(r => r.Ingredients).SetValidator(new IngredientValidator());
    }
  }

  public class RecipeStepValidator : AbstractValidator<RecipeStep>
  {
    public RecipeStepValidator()
    {
      RuleFor(rs => rs.Order)
        .NotNull()
        .GreaterThanOrEqualTo(1);

      RuleFor(rs => rs.Directions)
        .NotNull()
        .NotEmpty()
        .MaximumLength(300);

      RuleFor(rs => rs.ImageUrl)
        .NotEmpty()
        .When(rs => rs.ImageUrl != null);
    }
  }

  public class IngredientValidator : AbstractValidator<Ingredient>
  {
    public IngredientValidator()
    {
      RuleFor(i => i.Name)
        .NotNull()
        .NotEmpty()
        .MaximumLength(100);

      RuleFor(i => i.Amount)
        .NotNull()
        .NotEmpty()
        .MaximumLength(10);

      RuleFor(i => i.Unit)
        .NotNull()
        .NotEmpty()
        .MaximumLength(10);
    }
  }
}