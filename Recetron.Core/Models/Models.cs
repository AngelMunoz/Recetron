using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Recetron.Core.Models
{
  public record AuthResponse(string? Token, UserDTO User);

  public record ErrorResponse(string? Message)
  {
    public int? Code { get; init; }
    public IEnumerable<dynamic>? Errors { get; init; }
  }

  public record LoginPayload
  {
    [Required] [EmailAddress] public string? Email { get; set; }

    [Required] public string? Password { get; set; }
  }

  public readonly record struct PaginationResult<TType>(long Count, IEnumerable<TType> List);

  public record UserDTO(string? Id, string? Email, string? Name, string? LastName);

  public record SignUpPayload
  {
    [Required, MinLength(4)] public string? Email { get; set; }

    [Required,
     RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,30}$",
       ErrorMessage = "Password must contain at least 8 characters, a lowercase letter and an uppercase letter")]
    public string? Password { get; set; }

    [Required] public string? Name { get; set; }

    [Required] public string? LastName { get; set; }
  }

  public record Recipe
  {
    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string? Id { get; init; }

    public string? UserId { get; init; }

    [Required, MinLength(4)] public string? Title { get; set; }

    public string? ImageUrl { get; set; }

    [MaxLength(500)] public string? Description { get; set; }

    [MaxLength(240)] public string? Notes { get; set; }

    public IEnumerable<Ingredient> Ingredients { get; set; } = Array.Empty<Ingredient>();

    public IEnumerable<RecipeStep> Steps { get; set; } = Array.Empty<RecipeStep>();
  }

  public record RecipeStep
  {
    public int? Order { get; set; }

    [Required, MaxLength(300)] public string? Directions { get; set; }
    public string? ImageUrl { get; set; }
  }

  public record Ingredient
  {
    [Required, MaxLength(100)] public string? Name { get; set; }

    /// meaning how many/much of the measurment unit
    [Required, MaxLength(10)]
    public string? Amount { get; set; }

    /// what is the measurment unit? spoons, grams, ounces, 
    [Required, MaxLength(10)]
    public string? Unit { get; set; }

    public IEnumerable<Ingredient> Replacements { get; init; } = Array.Empty<Ingredient>();
  }
}