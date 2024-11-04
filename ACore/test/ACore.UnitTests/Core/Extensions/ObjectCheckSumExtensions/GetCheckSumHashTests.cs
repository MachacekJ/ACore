using ACore.Extensions;
using ACore.UnitTests.Core.Extensions.ObjectCheckSumExtensions.FakeData;
using FluentAssertions;

namespace ACore.UnitTests.Core.Extensions.ObjectCheckSumExtensions;

public class GetCheckSumHashTests
{
  [Theory] 
  [MemberData(nameof(Data))]
  public void TestCheckSumHashTest(FakeClassA class1, FakeClassA? class2, bool equal)
  {
    var hashClass1 = class1.GetSumHash();
    var hashClass2 = class2.GetSumHash();

    if (equal)
      hashClass1.Should().Be(hashClass2);
    else
      hashClass1.Should().NotBe(hashClass2);
  }

  public static IEnumerable<object?[]> Data =>
    new List<object?[]>
    {
      new object?[]
      {
        new FakeClassA
        {
          Int1 = 10,
          IntExclude = 11,
          String1 = "fake",
          StringExclude = "fakeA"
        },
        new FakeClassA
        {
          Int1 = 10,
          IntExclude = 12,
          String1 = "fake",
          StringExclude = "fakeB"
        },
        true
      },
      
      new object?[]
      {
        new FakeClassA
        {
          Int1 = 10,
          IntExclude = 11,
          String1 = "fake"
        },
        new FakeClassA
        {
          Int1 = 10,
          IntExclude = 12,
          StringExclude = "fake"
        },
        false
      },
      
      new object?[]
      {
        new FakeClassA
        {
          Int1 = 10,
          IntExclude = 11
        },
        new FakeClassA
        {
          Int1 = 10,
          IntExclude = 12,
          StringExclude = "fake"
        },
        true
      },
      new object?[]
      {
        new FakeClassA
        {
          Int1 = 10,
          IntExclude = 11
        },
        new FakeClassA
        {
          Int1 = 10,
          IntExclude = 12
        },
        true
      },
      new object?[]
      {
        new FakeClassA
        {
          Int1 = 10,
          IntExclude = 11
        },
        new FakeClassA
        {
          Int1 = 10,
          IntExclude = 11
        },
        true
      },
      
      new object?[]
      {
        new FakeClassA
        {
          Int1 = 10,
          IntExclude = 11
        },
        null,
        false
      },
    };
}