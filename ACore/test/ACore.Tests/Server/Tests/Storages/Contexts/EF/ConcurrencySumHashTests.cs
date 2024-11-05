using System.Reflection;
using ACore.Server.Storages.Contexts.EF.Models;
using ACore.Server.Storages.CQRS.Results;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Get;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Save;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKLong.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestPKLong.Save;
using ACore.Tests.Server.Tests.Modules.AuditModule.MemoryEF;
using FluentAssertions;
using Xunit;

namespace ACore.Tests.Server.Tests.Storages.Contexts.EF;

public class ConcurrencySumHashTests : AuditTestsBase
{
  private static readonly DateTime TestDateTime = new DateTime(2024, 1, 2).ToUniversalTime();
  private const string TestName = "AuditTest";

  [Fact]
  public async Task BasicTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Arrange
      var item = new TestNoAuditData<int>(TestName)
      {
        Created = TestDateTime,
      };

      // Action
      var result = await Mediator.Send(new TestNoAuditSaveCommand<int>(item, null)) as EntityResult;

      // Assert
      ArgumentNullException.ThrowIfNull(result);
      var hash = result.SingleDatabaseOperationResult().SumHash;
      hash.Should().NotBeNull();
    });
  }

  [Fact]
  public async Task BasicEmptyTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Arrange
      var item = new TestPKLongData
      {
        Created = TestDateTime,
        Name = "test"
      };

      // Action
      var result = await Mediator.Send(new TestPKLongSaveCommand(item)) as EntityResult;

      // Assert
      ArgumentNullException.ThrowIfNull(result);
      var hash = result.SingleDatabaseOperationResult().SumHash;
      hash.Should().BeNull();
    });
  }

  [Fact]
  public async Task ComplexTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      // Arrange
      var item = new TestNoAuditData<int>(TestName)
      {
        Created = TestDateTime,
      };

      // Action
      var result = await Mediator.Send(new TestNoAuditSaveCommand<int>(item, null)) as EntityResult;

      ArgumentNullException.ThrowIfNull(result);
      var hash = result.SingleDatabaseOperationResult().SumHash;
      hash.Should().NotBeNull();
      item.Id = result.SinglePrimaryKey<int>();

      var result2 = await Mediator.Send(new TestNoAuditSaveCommand<int>(item, hash)) as EntityResult;
      result2?.SingleDatabaseOperationResult().DatabaseOperationType.Should().Be(DatabaseOperationTypeEnum.UnModified);
      var hash2 = result2?.SingleDatabaseOperationResult().SumHash;
      hash2.Should().Be(hash);
      
      var allData = (await Mediator.Send(new TestNoAuditGetQuery<int>())).ResultValue;
      ArgumentNullException.ThrowIfNull(allData);
      allData.Should().HaveCount(1);
      var savedItem = allData.Single();
      savedItem.Key.Should().Be(hash2).And.Be(hash);

      item.Name = "faketest";
      var result3 = await Mediator.Send(new TestNoAuditSaveCommand<int>(item, hash)) as EntityResult;
      var hash3 = result3?.SingleDatabaseOperationResult().SumHash;
      savedItem.Key.Should().NotBe(hash3);
      hash3.Should().NotBe(hash);
    });
  }
}