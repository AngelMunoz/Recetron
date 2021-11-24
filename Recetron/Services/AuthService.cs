using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using JWT;
using Recetron.Interfaces;
using Recetron.Core.Models;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Recetron.Services
{
  public class AuthService : IAuthService
  {
    private readonly ISyncLocalStorageService _localstorage;
    private readonly IHttpClientFactory _httpFactory;
    private readonly IJSRuntime _js;

    public event EventHandler<bool>? AuthStateChanged;

    public AuthService(ISyncLocalStorageService localstorage, IHttpClientFactory client, IJSRuntime runtime)
    {
      this._localstorage = localstorage;
      _js = runtime;
      _httpFactory = client;
      this._localstorage.Changed += LocalStorageChanged;
    }

    public bool IsAuthenticated => CheckToken();

    public string Token => _localstorage.GetItem<string>(Constants.ACCESS_TOKEN_NAME);

    public async Task<bool> LoginAsync(LoginPayload payload)
    {
      using var http = _httpFactory.CreateClient(Constants.AUTH_CLIENT_NAME);
      try
      {
        var res = await http.PostAsJsonAsync($"{http.BaseAddress}/login", payload);
        if (res.IsSuccessStatusCode)
        {
          var auth = await res.Content.ReadFromJsonAsync<AuthResponse>();
          _localstorage.SetItem(Constants.ACCESS_TOKEN_NAME, auth.Token);
          _localstorage.SetItem(Constants.USER_DATA_NAME, auth.User);
        }
        else
        {
          var content = await res.Content.ReadFromJsonAsync<ErrorResponse>();
          Console.Error.WriteLine($"Failed to Login: {content?.Message}");
          return false;
        }
      }
      catch (Exception e)
      {
        Console.Error.WriteLine(e.Message);
        return false;
      }
      return true;
    }

    public async Task<bool> SignupAsync(SignUpPayload payload)
    {
      using var http = _httpFactory.CreateClient(Constants.AUTH_CLIENT_NAME);
      try
      {
        var res = await http.PostAsJsonAsync($"{http.BaseAddress}/signup", payload);
        if (res.IsSuccessStatusCode)
        {
          var auth = await res.Content.ReadFromJsonAsync<AuthResponse>();
          _localstorage.SetItem(Constants.ACCESS_TOKEN_NAME, auth?.Token);
          _localstorage.SetItem(Constants.USER_DATA_NAME, auth?.User);
        }
        else
        {
          var content = await res.Content.ReadFromJsonAsync<ErrorResponse>();
          Console.Error.WriteLine($"Failed to Login: {content?.Message}");
          return false;
        }
      }
      catch (System.Exception e)
      {
        Console.Error.WriteLine($"Error at signup {e.Message} - {e.StackTrace}");
        return false;
      }
      return true;
    }

    public Task LogoutAsync()
    {
      _localstorage.RemoveItem(Constants.ACCESS_TOKEN_NAME);
      _localstorage.RemoveItem(Constants.USER_DATA_NAME);
      return _js.InvokeVoidAsync("window.location.replace", "/").AsTask();
    }

    private void LocalStorageChanged(object? sender, ChangedEventArgs args)
    {
      if (args.Key != Constants.ACCESS_TOKEN_NAME) return;
      var isValidToken = CheckToken();
      AuthStateChanged?.Invoke(this, isValidToken);
    }

    private bool CheckToken()
    {
      string token = _localstorage.GetItem<string>(Constants.ACCESS_TOKEN_NAME);
      if (token is null or "null")
      {
        return false;
      }
      try
      {
        var decoder = new JwtDecoder(new CustomJsonSerializer(), new JwtBase64UrlEncoder());
        var data = decoder.DecodeToObject(token);
        data.TryGetValue("nbf", out object? _nbf);
        data.TryGetValue("exp", out object? _exp);

        if (_nbf != null && _exp != null)
        {
          var nbf = DateTimeOffset.FromUnixTimeSeconds((_nbf as JsonElement?).GetValueOrDefault().GetInt64());
          var exp = DateTimeOffset.FromUnixTimeSeconds((_exp as JsonElement?).GetValueOrDefault().GetInt64());
          var now = DateTimeOffset.Now;
          if (now < nbf) return false;
          if (now > exp) return false;
        }
        if (_exp != null && _nbf == null)
        {
          var exp = DateTimeOffset.FromUnixTimeSeconds((_exp as JsonElement?).GetValueOrDefault().GetInt64());
          var now = DateTimeOffset.Now;
          if (now > exp) return false;
        }
        if (_exp == null) return true;
      }
      catch (Exception e)
      {
        Console.Error.WriteLine($"Check Token Exception: {e.Message} - {e.StackTrace}");
        return false;
      }
      return true;
    }
  }
}