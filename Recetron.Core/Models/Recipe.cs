using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Recetron.Core.Models
{
  public class Recipe
  {

    [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
    public string? Id { get; set; }

    public string? UserId { get; set; }

    public string? Title { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public string? Notes { get; set; }

    public IEnumerable<Ingredient> Ingredients { get; set; } = new Ingredient[] { };

    public IEnumerable<RecipeStep> Steps { get; set; } = new RecipeStep[] { };

  }

  public class RecipeStep
  {
    public int? Order { get; set; }
    public string? Directions { get; set; }
    public string? ImageUrl { get; set; }

  }

  public class Ingredient
  {
    public string? Name { get; set; }

    /// meaning how many/much of the measurment unit
    public string? Amount { get; set; }

    /// what is the measurment unit? spoons, grams, ounces, 
    public string? Unit { get; set; }

    public IEnumerable<Ingredient> Replacements { get; set; } = new Ingredient[] { };
  }
}