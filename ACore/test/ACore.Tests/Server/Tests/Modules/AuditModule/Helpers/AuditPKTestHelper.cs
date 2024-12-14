using ACore.Results;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Get;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Save;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKGuid.Get;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKGuid.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKGuid.Save;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Get;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKLong.Save;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKString.Get;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKString.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1PKString.Save;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using FluentAssertions;
using MediatR;
using MongoDB.Bson;
using Fake1AuditEntity = ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models.Fake1AuditEntity;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.Helpers;

public static class AuditPKTestHelper
{
  private const string TestName = "AuditPK";
  private static readonly Type TestPKGuidEntityName = typeof(Fake1PKGuidEntity);
  private static readonly Type TestPKStringEntityName = typeof(Fake1PKStringEntity);
  private static readonly Type TestPKLongEntityName = typeof(Fake1PKLongEntity);
  private static readonly Type TestPKMongoEntityName = typeof(Fake1AuditEntity);
  private static readonly Type TestAuditEntityName = typeof(FakeApp.Modules.Fake1Module.Repositories.SQL.Models.Fake1AuditEntity);
  
  public static async Task IntPK(IMediator mediator, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    // Arrange.
    var item = new Fake1AuditData<int>
    {
      Name = TestName,
    };

    // Act.
    var result = await mediator.Send(new Fake1AuditSaveCommand<int>(item));

    // Assert.
    var allData = (await mediator.Send(new Fake1AuditGetQuery<int>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<Fake1AuditData<int>, int>(result, allData);
    
 
    // Assert.
    var resAuditItems = (await mediator.Send(new AuditGetQuery<int>(getTableName(TestAuditEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    var auditItem = resAuditItems.Single();
    var aid = auditItem.GetColumn(getColumnName(TestAuditEntityName, nameof(Fake1PKLongEntity.Id)));
    ArgumentNullException.ThrowIfNull(aid);
    aid.NewValue.Should().Be(itemId);
  }
  
  public static async Task LongPK(IMediator mediator, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    // Arrange.
    var item = new Fake1PKLongData
    {
      Name = TestName,
    };

    // Act.
    var result = await mediator.Send(new Fake1PKLongSaveCommand(item));

    // Assert.
    var allData = (await mediator.Send(new Fake1PKLongAuditGetQuery())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<Fake1PKLongData, long>(result, allData);
    
 
    // Assert.
    var resAuditItems = (await mediator.Send(new AuditGetQuery<long>(getTableName(TestPKLongEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    var auditItem = resAuditItems.Single();
    var aid = auditItem.GetColumn(getColumnName(TestPKLongEntityName, nameof(Fake1PKLongEntity.Id)));
    ArgumentNullException.ThrowIfNull(aid);
    aid.NewValue.Should().Be(itemId);
  }
  
  public static async Task GuidPK(IMediator mediator, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    // Arrange.
    var item = new Fake1PKGuidData
    {
      Name = TestName,
    };

    // Act.
    var result = await mediator.Send(new Fake1PKGuidSaveCommand(item));

    // Assert.
    var allData = (await mediator.Send(new Fake1PKGuidGetQuery())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<Fake1PKGuidData, Guid>(result, allData);

    var resAuditItems = (await mediator.Send(new AuditGetQuery<Guid>(getTableName(TestPKGuidEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    var auditItem = resAuditItems.Single();
    var aid = auditItem.GetColumn(getColumnName(TestPKGuidEntityName, nameof(Fake1PKGuidEntity.Id)));
    ArgumentNullException.ThrowIfNull(aid);
    aid.NewValue.Should().Be(itemId);
  }

  public static async Task StringPK(IMediator mediator, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    // Arrange.
    var item = new Fake1PKStringData
    {
      Name = TestName,
    };

    // Act.
    var result = await mediator.Send(new Fake1PKStringSaveCommand(item));

    // Assert.
    var allData = (await mediator.Send(new Fake1PKStringGetQuery())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<Fake1PKStringData, string>(result, allData);

    var resAuditItems = (await mediator.Send(new AuditGetQuery<string>(getTableName(TestPKStringEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    var auditItem = resAuditItems.Single();
    var aid = auditItem.GetColumn(getColumnName(TestPKStringEntityName, nameof(Fake1PKStringEntity.Id)));
    ArgumentNullException.ThrowIfNull(aid);
    aid.NewValue.Should().Be(itemId);
  }

  public static async Task ObjectIdPKNotImplemented(IMediator mediator)
  {
    var item = new Fake1AuditData<ObjectId>
    {
      Name = TestName,
    };

    // Act.
    var result = await mediator.Send(new Fake1AuditSaveCommand<ObjectId>(item));
    result.IsFailure.Should().BeTrue();
    result.ResultErrorItem.Code.Should().Be(ExceptionResult.ResultErrorItemInternalServer.Code);
    result.Should().BeOfType(typeof(ExceptionResult));
  }

  public static async Task ObjectIdPK(IMediator mediator, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    var item = new Fake1AuditData<ObjectId>
    {
      Name = TestName,
    };

    // Act.
    var result = await mediator.Send(new Fake1AuditSaveCommand<ObjectId>(item));

    // Assert.
    var allData = (await mediator.Send(new Fake1AuditGetQuery<ObjectId>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<Fake1AuditData<ObjectId>, ObjectId>(result, allData);
    
    // Assert.
    var resAuditItems = (await mediator.Send(new AuditGetQuery<ObjectId>(getTableName(TestPKMongoEntityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    var auditItem = resAuditItems.Single();
    var aid = auditItem.GetColumn(getColumnName(TestPKMongoEntityName, nameof(Fake1AuditEntity.Id)));
    ArgumentNullException.ThrowIfNull(aid);
    aid.NewValue.Should().Be(itemId);
  }
}
