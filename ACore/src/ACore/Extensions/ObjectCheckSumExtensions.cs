using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using ACore.Attributes;

namespace ACore.Extensions;

public static class ObjectCheckSumExtensions
{
  public static string GetSumHash(this object? serializableObject, string salt = "")
  {
    JsonSerializerOptions options = new()
    {
      TypeInfoResolver = new DefaultJsonTypeInfoResolver
      {
        Modifiers = { NotIncludeToCheckSumAttributeModifier },
      }
    };

    return serializableObject == null ? string.Empty : JsonSerializer.Serialize(serializableObject, options).HashString(salt);
  }
  
  private static void NotIncludeToCheckSumAttributeModifier(JsonTypeInfo jsonTypeInfo)
  {
    if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object)
      return;

    if (!jsonTypeInfo.Type.IsDefined(typeof(SumHashAttribute), inherit: false))
      return;

    var toRemove = new List<JsonPropertyInfo>();
    foreach (var propertyInfo in jsonTypeInfo.Properties)
    {
      if (propertyInfo.AttributeProvider != null
          && propertyInfo.AttributeProvider.IsDefined(typeof(NotIncludeToSumHashAttribute), true))
        toRemove.Add(propertyInfo);
    }
    
    jsonTypeInfo.Properties.RemoveAll(a=>toRemove.Contains(a));
  }
  private static void RemoveAll<T>(this IList<T> list, Predicate<T> predicate)
  {
    for (int i = 0; i < list.Count; i++)
    {
      if (predicate(list[i]))
      {
        list.RemoveAt(i--);
      }
    }
  }
  
  private static string HashString(this string text, string salt = "")
  {
    if (string.IsNullOrEmpty(text))
      return string.Empty;
    
    using var sha256 = SHA256.Create();

    // Convert the string to a byte array first, to be processed
    var textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
    var hashBytes = sha256.ComputeHash(textBytes);

    // Convert back to a string, removing the '-' that BitConverter adds
    string hash = BitConverter
      .ToString(hashBytes)
      .Replace("-", String.Empty);

    return hash;
  }
}