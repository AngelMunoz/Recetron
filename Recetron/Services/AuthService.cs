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
    private readonly ISyncLocalStorageService localstorage;
    private readonly IHttpClientFactory httpFactory;
    private readonly IJSRuntime js;

    public event EventHandler<bool>? AuthStateChanged;

    public AuthService(ISyncLocalStorageService localstorage, IHttpClientFactory client, IJSRuntime runtime)
    {
      this.localstorage = localstorage;
      js = runtime;
      httpFactory = client;
      this.localstorage.Changed += LocalStorageChanged;
    }

    public bool IsAuthenticated => CheckToken();

    public string Token => localstorage.GetItem<string>(Constants.ACCESS_TOKEN_NAME);

    public async Task<bool> LoginAsync(LoginPayload payload)
    {
      using var http = httpFactory.CreateClient(Constants.AUTH_CLIENT_NAME);
      try
      {
        var res = await http.PostAsJsonAsync($"{http.BaseAddress}/login", payload);
        if (res.IsSuccessStatusCode)
        {
          var auth = await res.Content.ReadFromJsonAsync<AuthResponse>();
          localstorage.SetItem(Constants.ACCESS_TOKEN_NAME, auth.Token);
          localstorage.SetItem(Constants.USER_DATA_NAME, auth.User);
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
      using var http = httpFactory.CreateClient(Constants.AUTH_CLIENT_NAME);
      try
      {
        var res = await http.PostAsJsonAsync<SignUpPayload>($"{http.BaseAddress}/signup", payload);
        if (res.IsSuccessStatusCode)
        {
          var auth = await res.Content.ReadFromJsonAsync<AuthResponse>();
          localstorage.SetItem(Constants.ACCESS_TOKEN_NAME, auth.Token);
          localstorage.SetItem(Constants.USER_DATA_NAME, auth.User);
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
        Console.Error.WriteLine($"Error at signup {e.Message} - {e.StackTrace}");
        return false;
      }
      return true;
    }

    public Task LogoutAsync()
    {
      localstorage.RemoveItem(Constants.ACCESS_TOKEN_NAME);
      localstorage.RemoveItem(Constants.USER_DATA_NAME);
      return js.InvokeVoidAsync("window.location.replace", "/").AsTask();
    }

    private void LocalStorageChanged(object sender, ChangedEventArgs args)
    {
      if (args.Key != Constants.ACCESS_TOKEN_NAME) return;
      var isValidToken = CheckToken();
      AuthStateChanged?.Invoke(this, isValidToken);
    }

    private bool CheckToken()
    {
      string token = localstorage.GetItem<string>(Constants.ACCESS_TOKEN_NAME);
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