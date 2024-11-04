using ACore.Attributes;

namespace ACore.UnitTests.Core.Extensions.ObjectCheckSumExtensions.FakeData;

[SumHash]
public class FakeClassA
{
  public string String1 { get; set; } = string.Empty;
  public int Int1 { get; set; }

  [NotIncludeToSumHash]
  public string StringExclude { get; set; } = string.Empty;

  [NotIncludeToSumHash]
  public int IntExclude { get; set; }
}