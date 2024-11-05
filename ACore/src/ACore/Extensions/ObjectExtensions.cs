using System.Reflection;
using ACore.Extensions.Models;

namespace ACore.Extensions;

public static class ObjectExtensions
{
  public static object? PropertyValue(this object self, string propertyName)
    => GetProperty(self, propertyName)?.GetValue(self);

  private static PropertyInfo? GetProperty(this object self, string propertyName)
    => self.GetType().GetProperty(propertyName);

  /// <summary>
  /// customCompare is not null e.g. Bson.ObjectId namespace is not registered in <see cref="ACore"/> but I need to compare it from server where is registered.
  /// </summary>
  public static ComparisonResultData[] Compare<T>(this T leftObj, T? rightObj, Func<object, object, bool?>? customCompare = null, string? parentName = null)
    where T : class
  {
    var results = new List<ComparisonResultData>();
    var rightProperties = rightObj == null ? null : GetProperties(rightObj);
    var leftProperties = GetProperties(leftObj);

    foreach (var leftProperty in leftProperties)
    {
      var leftValue = leftProperty.GetValue(leftObj);

      if (rightProperties == null)
      {
        results.Add(new ComparisonResultData(leftProperty.Name, leftProperty.PropertyType, true, leftValue, null));
        continue;
      }

      foreach (var newProperty in rightProperties)
      {
        if (leftProperty.Name != newProperty.Name)
          continue;

        if (leftProperty.PropertyType != newProperty.PropertyType)
          continue;

        var rightValue = newProperty.GetValue(rightObj);

        var isChange = (rightValue == null && leftValue != null)
                       || (rightValue != null && leftValue == null);

        if (!isChange && rightValue != null && leftValue != null)
          isChange = CompareValue(leftValue, rightValue, customCompare);

        results.Add(new ComparisonResultData(leftProperty.Name, leftProperty.PropertyType, isChange, leftValue, rightValue));
        break;
      }
    }

    return results.ToArray();
  }

  private static bool CompareValue(object leftValue, object rightValue, Func<object, object, bool?>? customCompare = null)
  {
    var res = customCompare?.Invoke(leftValue, rightValue);
    if (res != null)
      return res.Value;

    bool isChange;
    if (rightValue is byte[] enumRight && leftValue is byte[] enumLeft)
      isChange = !enumRight.SequenceEqual(enumLeft);
    else
      isChange = !rightValue.Equals(leftValue);
    return isChange;
  }

  private static PropertyInfo[] GetProperties(object obj)
    => obj.GetType().GetProperties();
}