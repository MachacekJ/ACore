namespace ACore.Attributes;

public static class CheckSumAttributesExtensions
{
  public static bool IsSumHashAllowed(this Type item)
  {
    var enableCheckSumAttribute = Attribute.GetCustomAttribute(item, typeof(SumHashAttribute));
    return enableCheckSumAttribute is SumHashAttribute;
  }
}