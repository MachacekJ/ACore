namespace ACore.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SumHashAttribute : Attribute;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class NotIncludeToSumHashAttribute : Attribute;