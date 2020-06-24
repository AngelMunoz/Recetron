using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using JWT;
using JWT.Builder;
using JWT.Serializers;
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
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    public event EventHandler<bool>? AuthStateChanged;

    public AuthService(ISyncLocalStorageService localstorage, IHttpClientFactory client, IJSRuntime runtime)
    {
      _localstorage = localstorage;
      _js = runtime;
      _http = client.CreateClient(Constants.AUTH_CLIENT_NAME);
      _localstorage.Changed += LocalStorageChanged;
    }

    public bool IsAuthenticated => CheckToken();

    public string Token => _localstorage.GetItem<string>(Constants.ACCESS_TOKEN_NAME);

    public async Task<bool> LoginAsync(LoginPayload payload)
    {
      try
      {
        var res = await _http.PostAsJsonAsync<LoginPayload>($"{_http.BaseAddress}/login", payload);
        if (res.IsSuccessStatusCode)
        {
          var auth = await res.Content.ReadFromJsonAsync<AuthResponse>();
          _localstorage.SetItem(Constants.ACCESS_TOKEN_NAME, auth.Token);
          _localstorage.SetItem(Constants.USER_DATA_NAME, auth.User);
        }
        else
        {
          var content = await res.Content.ReadFromJsonAsync<ErrorResponse>();
          Console.Error.WriteLine($"Failed to Login: {content.Message}");
          return false;
        }
      }
      catch (System.Exception e)
      {
        Console.Error.WriteLine(e.Message);
        return false;
      }
      return true;
    }

    public async Task<bool> SignupAsync(SignUpPayload payload)
    {
      try
      {
        var res = await _http.PostAsJsonAsync<SignUpPayload>($"{_http.BaseAddress}/signup", payload);
        if (res.IsSuccessStatusCode)
        {
          var auth = await res.Content.ReadFromJsonAsync<AuthResponse>();
          _localstorage.SetItem(Constants.ACCESS_TOKEN_NAME, auth.Token);
          _localstorage.SetItem(Constants.USER_DATA_NAME, auth.User);
        }
        else
        {
          var content = await res.Content.ReadFromJsonAsync<ErrorResponse>();
          Console.Error.WriteLine($"Failed to Login: {content.Message}");
          return false;
        }
      }
      catch (System.Exception e)
      {
        Console.Error.WriteLine(e.Message);
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

    private void LocalStorageChanged(object sender, ChangedEventArgs args)
    {
      if (args.Key != Constants.ACCESS_TOKEN_NAME) return;
      var isValidToken = CheckToken();
      AuthStateChanged?.Invoke(this, isValidToken);
    }

    private bool CheckToken()
    {
      string token = _localstorage.GetItem<string>(Constants.ACCESS_TOKEN_NAME);
      if (token is null || token == "null")
      {
        return false;
      }
      try
      {
        var decoder = new JwtDecoder(new CustomJsonSerializer(), new JwtBase64UrlEncoder());
        var data = decoder.DecodeToObject(token);
        data.TryGetValue("nbf", out object _nbf);
        data.TryGetValue("exp", out object _exp);

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
      catch (System.Exception e)
      {
        Console.Error.WriteLine($"Check Token Exception: {e.Message} - {e.StackTrace}");
        return false;
      }
      return true;
    }
  }
}