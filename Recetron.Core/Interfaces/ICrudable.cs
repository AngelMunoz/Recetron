using System.Threading.Tasks;
using Recetron.Core.Models;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Linq.Expressions;

namespace Recetron.Core.Interfaces
{
  public interface ICrudable<T>
  {
    Task<PaginationResult<T>> Find(int page, int limit, CancellationToken ct = default);

    Task<T> FindOne(string id, CancellationToken ct = default);

    Task<T> Create(T item, CancellationToken ct = default);

    Task<bool> Update(T item, CancellationToken ct = default);

    Task<bool> Destroy(string id, CancellationToken ct = default);

  }
}