using JWT;
using System.Text.Json;

namespace Recetron
{
  public class CustomJsonSerializer : IJsonSerializer
  {
    private readonly JsonSerializerOptions _options =
      new JsonSerializerOptions
      {
        AllowTrailingCommas = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        ReadCommentHandling = JsonCommentHandling.Skip,
      };
    public T Deserialize<T>(string json)
    {
      return JsonSerializer.Deserialize<T>(json, _options);
    }

    public string Serialize(object obj)
    {
      return JsonSerializer.Serialize(obj, _options);
    }
  }
}