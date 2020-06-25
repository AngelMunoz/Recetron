using System.Collections.Generic;

namespace Recetron.Core.Models
{
  public class ErrorResponse
  {
    public string? Message { get; set; }
    public int? Code { get; set; }
    public IEnumerable<dynamic> Errors { get; set; } = new dynamic[] { };
  }
}