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

    private readonly IHttpClientFactory _httpFactory;

    public RecipeService(IHttpClientFactory clientFactory)
    {
      _httpFactory = clientFactory;
    }

    public async Task<Recipe> Create(Recipe item, CancellationToken ct = default)
    {
      using var http = _httpFactory.CreateClient(Constants.API_CLIENT_NAME);
      var res = await http.PostAsJsonAsync($"{http.BaseAddress}/recipes", item, cancellationToken: ct);
      if (!res.IsSuccessStatusCode)
      {
        var response = await res.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: ct);
        throw new ArgumentException(response?.Message);
      }
      var result = await res.Content.ReadFromJsonAsync<Recipe>(cancellationToken: ct);
      if (result is null)
      {
        throw new AggregateException("The Recipe could not be de-serialized into json");
      }
      return result;

    }

    public async Task<bool> Destroy(string id, CancellationToken ct = default)
    {
      using var http = _httpFactory.CreateClient(Constants.API_CLIENT_NAME);
      var res = await http.DeleteAsync($"{http.BaseAddress}/recipes/{id}", cancellationToken: ct);
      return res.IsSuccessStatusCode;
    }

    public async Task<PaginationResult<Recipe>> Find(int page = 1, int limit = 10, CancellationToken ct = default)
    {
      using var http = _httpFactory.CreateClient(Constants.API_CLIENT_NAME);
      var uri = new Uri($"{http.BaseAddress}/recipes?page={page}&limit={limit}");
      return await http.GetFromJsonAsync<PaginationResult<Recipe>>(uri, cancellationToken: ct);
    }

    public async Task<IEnumerable<Recipe>> FindByNameAsync(string recipeName, CancellationToken ct = default)
    {
      using var http = _httpFactory.CreateClient(Constants.API_CLIENT_NAME);
      var uri = new Uri($"{http.BaseAddress}/recipes?searchByName={recipeName}");
      var res = await http.GetFromJsonAsync<IEnumerable<Recipe>>(uri, cancellationToken: ct);
      if (res is null)
      {
        throw new AggregateException("The Recipe could not be de-serialized into json");
      }
      return res;
    }

    /// This method is not used in the UI please use <see cref="Recetron.Services.RecipeService.Find(int, int, CancellationToken)"/>
    [Obsolete("This method is not used in the UI, please use Find", true)]
    public Task<PaginationResult<Recipe>> FindByUser(string userId, int page, int limit, CancellationToken ct = default)
    {
      throw new NotImplementedException();
    }

    public async Task<Recipe> FindOne(string id, CancellationToken ct = default)
    {
      using var http = _httpFactory.CreateClient(Constants.API_CLIENT_NAME);
      var uri = new Uri($"{http.BaseAddress}/recipes/{id}");
      var res = await http.GetFromJsonAsync<Recipe>(uri, cancellationToken: ct);
      if (res is null)
      {
        throw new AggregateException("The Recipe could not be de-serialized into json");
      }
      return res;
    }

    public async Task<bool> Update(Recipe item, CancellationToken ct = default)
    {
      using var http = _httpFactory.CreateClient(Constants.API_CLIENT_NAME);
      var res = await http.PutAsJsonAsync<Recipe>($"{http.BaseAddress}/recipes", item, cancellationToken: ct);
      
      if (res.IsSuccessStatusCode)
        return await res.Content.ReadFromJsonAsync<bool>(cancellationToken: ct);
      
      var response = await res.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: ct);
      throw new ArgumentException(response?.Message);
    }
  }
}