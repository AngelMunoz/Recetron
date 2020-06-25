using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Blazored.LocalStorage;
using Recetron.Interfaces;
using Recetron.Services;

namespace Recetron
{
  public static class Constants
  {
    public static readonly string AUTH_CLIENT_NAME = "AuthApi";

    public static readonly string API_CLIENT_NAME = "Api";

    public static readonly string ACCESS_TOKEN_NAME = "access_token";

    public static readonly string USER_DATA_NAME = "user_data";

    public static readonly string AUTH_URL = "/auth";

    public static readonly string RECIPES_URL = "/recipes";

    public static readonly string NOTES_URL = "/notes";
  }

}
