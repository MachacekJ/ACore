using ACore.Extensions;
using ACore.Extensions.Models;
using ACore.UnitTests.Core.Extensions.ObjectExtensions.FakeData;
using FluentAssertions;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace ACore.UnitTests.Core.Extensions.ObjectExtensions;

public class CompareTests
{
  private static readonly DateTime TestDate = new(2020, 12, 23);
  private static readonly Guid TestGuid = Guid.NewGuid();
  private static readonly byte[] TestByteArray = "Hello World"u8.ToArray();
  private static ObjectId _testObjectId = ObjectId.GenerateNewId();

  [Theory]
  [MemberData(nameof(Data))]
  public void BaseTest(Fake1Class oldObject, Fake1Class? newObject, ComparisonResultData[] expectedResults)
  {
    // Act
    var res = oldObject.Compare(newObject);

    // Assert
    res.Should().HaveCount(expectedResults.Length);
    for (var i = 0; i < res.Length; i++)
    {
      CompareResult(res[i], expectedResults[i]);
    }
  }

  private void CompareResult(ComparisonResultData resultData, ComparisonResultData expectedResultData)
  {
    resultData.Name.Should().Be(expectedResultData.Name);
    resultData.Type.Should().Be(expectedResultData.Type);
    resultData.IsChange.Should().Be(expectedResultData.IsChange);

    if (resultData.LeftValue == null)
      expectedResultData.LeftValue.Should().BeNull();

    var lrv = JsonConvert.SerializeObject(resultData.LeftValue);
    var lev = JsonConvert.SerializeObject(expectedResultData.LeftValue);
    lrv.Should().Be(lev);

    if (resultData.RightValue == null)
      expectedResultData.RightValue.Should().BeNull();

    var rrv = JsonConvert.SerializeObject(resultData.RightValue);
    var rev = JsonConvert.SerializeObject(expectedResultData.RightValue);
    rrv.Should().Be(rev);
  }

  public static IEnumerable<object?[]> Data =>
    new List<object?[]>
    {
      new object?[]
      {
        new Fake1Class(), null, BaseComparisionWithEx([], true)
      },
      new object?[]
      {
        new Fake1Class(), new Fake1Class(), BaseComparisionWithEx([], false),
      },
      new object?[]
      {
        new Fake1Class { Int = 1 }, new Fake1Class(), BaseComparisionWithEx([
          new ComparisonResultData(nameof(Fake1Class.Int), typeof(int?), true, 1, null)
        ], false),
      },
      new object?[]
      {
        new Fake1Class(), new Fake1Class { Int = 1 }, BaseComparisionWithEx([
          new ComparisonResultData(nameof(Fake1Class.Int), typeof(int?), true, null, 1)
        ], false),
      },
      new object?[]
      {
        new Fake1Class { Int = 1 }, new Fake1Class { Int = 1 }, BaseComparisionWithEx([
          new ComparisonResultData(nameof(Fake1Class.Int), typeof(int?), false, 1, 1)
        ], false),
      },
      new object?[]
      {
        new Fake1Class { DateTime = TestDate }, new Fake1Class { DateTime = TestDate }, BaseComparisionWithEx([
          new ComparisonResultData(nameof(Fake1Class.DateTime), typeof(DateTime?), false, TestDate, TestDate)
        ], false),
      },
      new object?[]
      {
        new Fake1Class { Guid = TestGuid }, new Fake1Class { Guid = TestGuid }, BaseComparisionWithEx([
          new ComparisonResultData(nameof(Fake1Class.Guid), typeof(Guid?), false, TestGuid, TestGuid)
        ], false),
      },
      new object?[]
      {
        new Fake1Class { ByteArr = TestByteArray }, new Fake1Class { ByteArr = new List<byte>(TestByteArray).ToArray() }, BaseComparisionWithEx([
          new ComparisonResultData(nameof(Fake1Class.ByteArr), typeof(byte[]), false, new List<byte>(TestByteArray).ToArray(), new List<byte>(TestByteArray).ToArray())
        ], false),
      },
      new object?[]
      {
        new Fake1Class { String = "fake" }, new Fake1Class { String = "fake" }, BaseComparisionWithEx([
          new ComparisonResultData(nameof(Fake1Class.String), typeof(string), false, "fake", "fake")
        ], false),
      },
      new object?[]
      {
        new Fake1Class { MongoId = _testObjectId }, new Fake1Class { MongoId = new ObjectId(_testObjectId.ToByteArray()) }, BaseComparisionWithEx([
          new ComparisonResultData(nameof(Fake1Class.MongoId), typeof(ObjectId?), false, new ObjectId(_testObjectId.ToByteArray()), new ObjectId(_testObjectId.ToByteArray()))
        ], false),
      },
      new object?[]
      {
        new Fake1Class { MongoId = _testObjectId }, new Fake1Class { DateTime = TestDate }, BaseComparisionWithEx([
          new ComparisonResultData(nameof(Fake1Class.MongoId), typeof(ObjectId?), true, new ObjectId(_testObjectId.ToByteArray()), null),
          new ComparisonResultData(nameof(Fake1Class.DateTime), typeof(DateTime?), true, null, TestDate)
        ], false),
      },
    };

  private static List<ComparisonResultData> BaseComparisionWithEx(List<ComparisonResultData> results, bool isChange)
  {
    var baseR = new List<ComparisonResultData>();

    var single = results.SingleOrDefault(e => e.Name == nameof(Fake1Class.Int));
    baseR.Add(single ?? new ComparisonResultData(nameof(Fake1Class.Int), typeof(int?), isChange, null, null));

    single = results.SingleOrDefault(e => e.Name == nameof(Fake1Class.DateTime));
    baseR.Add(single ?? new ComparisonResultData(nameof(Fake1Class.DateTime), typeof(DateTime?), isChange, null, null));

    single = results.SingleOrDefault(e => e.Name == nameof(Fake1Class.Guid));
    baseR.Add(single ?? new ComparisonResultData(nameof(Fake1Class.Guid), typeof(Guid?), isChange, null, null));

    single = results.SingleOrDefault(e => e.Name == nameof(Fake1Class.ByteArr));
    baseR.Add(single ?? new ComparisonResultData(nameof(Fake1Class.ByteArr), typeof(byte[]), isChange, null, null));

    single = results.SingleOrDefault(e => e.Name == nameof(Fake1Class.String));
    baseR.Add(single ?? new(nameof(Fake1Class.String), typeof(string), isChange, null, null));

    single = results.SingleOrDefault(e => e.Name == nameof(Fake1Class.MongoId));
    baseR.Add(single ?? new(nameof(Fake1Class.MongoId), typeof(ObjectId?), isChange, null, null));

    return baseR;
  }
}