using System.Collections.Generic;

namespace Recetron.Core.Models
{
  public class PaginationResult<T>
  {
    public long Count { get; set; } = 0;
    public IEnumerable<T> List { get; set; } = new T[] { };
  }
}