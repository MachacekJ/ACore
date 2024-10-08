using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using ACore.Extensions;
using MongoDB.Bson;

namespace ACore.Server.Modules.AuditModule.Storage.Helpers;

public static class AuditValueObjectConverter
{
  public static string? ToAuditValue(this object? value)
  {
    if (value == null)
      return null;

    return value switch
    {
      string or byte or short or int or long or bool or Guid or ObjectId => value.ToString(),
      TimeSpan ts => ts.Ticks.ToString(),
      DateTime dateTime => dateTime.Ticks.ToString(),
      decimal dec => dec.ToString(CultureInfo.InvariantCulture),
      byte[] => JsonSerializer.Serialize(value),
      _ => throw new Exception($"Unknown type for audit. Type: {value.GetType().ACoreTypeName()}; Value:{value}")
    };
  }

  public static object? ConvertObjectToDataType(this object? value, string dataType)
  {
    if (value == null)
      return null;

    if (string.IsNullOrEmpty(dataType))
      throw new ArgumentNullException($"Data type is null.");

    if (dataType == typeof(ObjectId).ACoreTypeName())
      return new ObjectId(value.ToString());

    if (dataType == typeof(Guid).ACoreTypeName())
      return new Guid(value.ToString() ?? throw new InvalidOperationException());

    if (dataType == typeof(DateTime).ACoreTypeName())
      return new DateTime(Convert.ToInt64(value), DateTimeKind.Utc);

    var type = Type.GetType(dataType);
    if (dataType == typeof(byte[]).ACoreTypeName() && type != null)
      return JsonSerializer.Deserialize(value.ToString() ?? throw new NullReferenceException(), type);

    if (type == null)
      throw new Exception($"Cannot create data type '{dataType}'.");

    var c = ChangeType(value, type);
    return c;
  }

  private static object ChangeType(object value, Type conversionType)
  {
    ArgumentNullException.ThrowIfNull(conversionType);

    if (conversionType.IsGenericType &&
        conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
    {
      if (value == null)
      {
        return null;
      }

      NullableConverter nullableConverter = new NullableConverter(conversionType);
      conversionType = nullableConverter.UnderlyingType;
    }

    return Convert.ChangeType(value, conversionType);
  }
}