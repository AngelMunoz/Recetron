using System;
using System.Threading.Tasks;
using Recetron.Core.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Collections.Generic;
using Recetron.Core.Interfaces;

namespace Recetron.Services
{
  public class RecipeService : IRecipeService
  {

    private readonly HttpClient _http;

    public RecipeService(IHttpClientFactory clientFactory)
    {
      _http = clientFactory.CreateClient(Constants.API_CLIENT_NAME);
    }

    public async Task<Recipe> Create(Recipe item, CancellationToken ct = default)
    {
      var res = await _http.PostAsJsonAsync<Recipe>($"{_http.BaseAddress}/recipes", item, cancellationToken: ct);
      if (!res.IsSuccessStatusCode)
      {
        var response = await res.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: ct);
        throw new ArgumentException(response.Message);
      }
      return await res.Content.ReadFromJsonAsync<Recipe>(cancellationToken: ct);
    }

    public Task<bool> Destroy(string id, CancellationToken ct = default)
    {
      return _http
        .DeleteAsync($"{_http.BaseAddress}/recipes/{id}", cancellationToken: ct)
        .ContinueWith(res => res.Result.IsSuccessStatusCode, cancellationToken: ct);
    }

    public Task<PaginationResult<Recipe>> Find(int page = 1, int limit = 10, CancellationToken ct = default)
    {
      var uri = new Uri($"{_http.BaseAddress}/recipes?page={page}&limit={limit}");
      return _http.GetFromJsonAsync<PaginationResult<Recipe>>(uri, cancellationToken: ct);
    }

    public Task<IEnumerable<Recipe>> FindByNameAsync(string recipeName, CancellationToken ct = default)
    {
      var uri = new Uri($"{_http.BaseAddress}/recipes?searchByName={recipeName}");
      return _http.GetFromJsonAsync<IEnumerable<Recipe>>(uri, cancellationToken: ct);
    }

    /// This method is not used in the UI please use <see cref="Recetron.Services.RecipeService.Find(int, int, CancellationToken)"/>
    [Obsolete("This method is not used in the UI, please use Find", true)]
    public Task<PaginationResult<Recipe>> FindByUser(string userId, int page, int limit, CancellationToken ct = default)
    {
      throw new NotImplementedException();
    }

    public Task<Recipe> FindOne(string id, CancellationToken ct = default)
    {
      var uri = new Uri($"{_http.BaseAddress}/recipes/{id}");
      return _http.GetFromJsonAsync<Recipe>(uri, cancellationToken: ct);
    }

    public async Task<bool> Update(Recipe item, CancellationToken ct = default)
    {
      var res = await _http.PutAsJsonAsync<Recipe>($"{_http.BaseAddress}/recipes", item, cancellationToken: ct);
      if (!res.IsSuccessStatusCode)
      {
        var response = await res.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: ct);
        throw new ArgumentException(response.Message);
      }
      return await res.Content.ReadFromJsonAsync<bool>(cancellationToken: ct);
    }
  }
}