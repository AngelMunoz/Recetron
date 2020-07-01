using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Recetron.Core.Models
{
  public class Recipe
  {

    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string? Id { get; set; }

    public string? UserId { get; set; }

    [Required, MinLength(4)]
    public string? Title { get; set; }

    public string? ImageUrl { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(240)]
    public string? Notes { get; set; }

    public IEnumerable<Ingredient> Ingredients { get; set; } = new Ingredient[] { };

    public IEnumerable<RecipeStep> Steps { get; set; } = new RecipeStep[] { };

  }

  public class RecipeStep
  {
    public int? Order { get; set; }

    [Required, MaxLength(300)]
    public string? Directions { get; set; }
    public string? ImageUrl { get; set; }

  }

  public class Ingredient
  {
    [Required, MaxLength(100)]
    public string? Name { get; set; }

    /// meaning how many/much of the measurment unit
    [Required, MaxLength(10)]
    public string? Amount { get; set; }

    /// what is the measurment unit? spoons, grams, ounces, 
    [Required, MaxLength(10)]
    public string? Unit { get; set; }

    public IEnumerable<Ingredient> Replacements { get; set; } = new Ingredient[] { };
  }
}