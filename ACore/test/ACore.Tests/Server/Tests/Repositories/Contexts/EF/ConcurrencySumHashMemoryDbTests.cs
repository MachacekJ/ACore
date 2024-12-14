using System.Reflection;
using ACore.Server.Repository.Results;
using ACore.Server.Repository.Results.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Get;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Save;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Save;
using ACore.Tests.Server.Tests.Modules.AuditModule.MemoryEF;
using FluentAssertions;
using Xunit;

namespace ACore.Tests.Server.Tests.Repositories.Contexts.EF;

public class ConcurrencySumHashMemoryDbTests : AuditMemoryDbTestsBase
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
      var item = new Fake1NoAuditData<int>(TestName)
      {
        Created = TestDateTime,
      };

      // Action
      var result = await Mediator.Send(new Fake1NoAuditSaveCommand<int>(item, null)) as RepositoryResult;

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
      var item = new Fake1PKLongData
      {
        Created = TestDateTime,
        Name = "test"
      };

      // Action
      var result = await Mediator.Send(new Fake1PKLongSaveCommand(item)) as RepositoryResult;

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
      var item = new Fake1NoAuditData<int>(TestName)
      {
        Created = TestDateTime,
      };

      // Action
      var result = await Mediator.Send(new Fake1NoAuditSaveCommand<int>(item, null)) as RepositoryResult;

      ArgumentNullException.ThrowIfNull(result);
      var hash = result.SingleDatabaseOperationResult().SumHash;
      hash.Should().NotBeNull();
      item.Id = result.SinglePrimaryKey<int>();

      var result2 = await Mediator.Send(new Fake1NoAuditSaveCommand<int>(item, hash)) as RepositoryResult;
      result2?.SingleDatabaseOperationResult().RepositoryOperationType.Should().Be(RepositoryOperationTypeEnum.UnModified);
      var hash2 = result2?.SingleDatabaseOperationResult().SumHash;
      hash2.Should().Be(hash);
      
      var allData = (await Mediator.Send(new Fake1NoAuditGetQuery<int>())).ResultValue;
      ArgumentNullException.ThrowIfNull(allData);
      allData.Should().HaveCount(1);
      var savedItem = allData.Single();
      savedItem.Key.Should().Be(hash2).And.Be(hash);

      item.Name = "faketest";
      var result3 = await Mediator.Send(new Fake1NoAuditSaveCommand<int>(item, hash)) as RepositoryResult;
      var hash3 = result3?.SingleDatabaseOperationResult().SumHash;
      savedItem.Key.Should().NotBe(hash3);
      hash3.Should().NotBe(hash);
    });
  }
}