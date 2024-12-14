using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet.Models;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Repository.Contexts.Helpers;
using ACore.Server.Repository.Contexts.Mongo.Models.PK;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Delete;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Get;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1Audit.Save;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Get;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Save;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using FluentAssertions;
using MediatR;
using MongoDB.Bson;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.Helpers;

public static class AuditCRUDTestHelper
{
  private static readonly DateTime TestDateTime = DateTime.UtcNow;
  private const string TestName = "AuditTest";
  private const string TestNameUpdate = "AuditTestUpdate";
  private static readonly Type TestAuditEntityName = typeof(Fake1AuditEntity);
  private static readonly Type TestNoAuditEntityName = typeof(Fake1NoAuditEntity);
  private static readonly Type TestAuditMongoEntityName = typeof(ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models.Fake1AuditEntity);
  private static readonly Type TestNoAuditMongoEntityName = typeof(ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models.Fake1NoAuditEntity);

  public static async Task NoAuditAsyncTest<TPK>(IMediator mediator, Func<Type, string> getTableName)
  {
    var testNoAuditEntityType = typeof(TPK) == typeof(ObjectId) ? TestNoAuditMongoEntityName : TestNoAuditEntityName;
    // Arrange
    var item = new Fake1NoAuditData<TPK>(TestName)
    {
      Created = TestDateTime,
    };

    // Action
    var result = await mediator.Send(new Fake1NoAuditSaveCommand<TPK>(item, null));

    // Assert
    var allData = (await mediator.Send(new Fake1NoAuditGetQuery<TPK>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<Fake1NoAuditData<TPK>, TPK>(result, allData?.Values.ToArray());
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TPK>(getTableName(testNoAuditEntityType), itemId))).ResultValue;

    resAuditItems.Should().HaveCount(0);
  }

  public static async Task AddItemAsyncTest<TPK>(IMediator mediator, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    // Arrange
    var testAuditEntityType = typeof(TPK) == typeof(ObjectId) ? TestAuditMongoEntityName : TestAuditEntityName;
    var item = new Fake1AuditData<TPK>
    {
      Created = TestDateTime,
      Name = TestName,
      NotAuditableColumn = "Audit"
    };

    // Action.
    var result = await mediator.Send(new Fake1AuditSaveCommand<TPK>(item));

    // Assert.
    var allData = (await mediator.Send(new Fake1AuditGetQuery<TPK>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<Fake1AuditData<TPK>, TPK>(result, allData);
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TPK>(getTableName(testAuditEntityType), itemId))).ResultValue;

    resAuditItems.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(1);
    resAuditItems.Single().State.Should().Be(AuditInfoStateEnum.Added);

    var auditItem = resAuditItems.Single();
    // All items in mongo have the Version property added.
    CheckCount(auditItem);
    auditItem.Columns.All(c => c.IsChange).Should().Be(true);

    var noAuditableColumn = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.NotAuditableColumn)));
    var aid = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.Id)));
    var aName = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.Created)));

    noAuditableColumn.Should().BeNull();

    aid.Should().NotBeNull();
    aName.Should().NotBeNull();
    aName.NewValue.Should().NotBeNull();

    aCreated.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aCreated);
    aCreated.NewValue.Should().NotBeNull();
    aCreated.DataType.ToLower().Should().NotContain("string");

    aid.NewValue.Should().Be(itemId);
    aName?.NewValue.Should().Be(TestName);
    aCreated.NewValue.Should().Be(TestDateTime);
  }


  public static async Task UpdateItemAsyncTest<TPK>(IMediator mediator, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    var testAuditEntityType = typeof(TPK) == typeof(ObjectId) ? TestAuditMongoEntityName : TestAuditEntityName;
    // Arrange
    var item = new Fake1AuditData<TPK>
    {
      Created = TestDateTime,
      Name = TestName,
      NullValue = TestName,
      NullValue2 = null,
      NotAuditableColumn = "Audit"
    };

    // Act.
    var result = await mediator.Send(new Fake1AuditSaveCommand<TPK>(item));

    var allData = (await mediator.Send(new Fake1AuditGetQuery<TPK>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<Fake1AuditData<TPK>, TPK>(result, allData);

    item.Id = itemId;
    item.Name = TestNameUpdate;
    item.NullValue = null;
    item.NullValue2 = TestNameUpdate;

    // Update
    await mediator.Send(new Fake1AuditSaveCommand<TPK>(item));

    // Assert.
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TPK>(getTableName(testAuditEntityType), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(2);
    resAuditItems.Last().State.Should().Be(AuditInfoStateEnum.Modified);


    var auditItem = resAuditItems.Single(a => a.State == AuditInfoStateEnum.Modified);
    // only Name was changed
    CheckCount(auditItem);

    var aid = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.Id)));
    var aName = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.Created)));
    var aNullValue = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.NullValue)));
    var aNullValue2 = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.NullValue2)));
    var aNullValue3 = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.NullValue3)));

    aid.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aid);
    aid.IsChange.Should().BeFalse();
    aid.OldValue.Should().Be(itemId);
    aid.NewValue.Should().BeNull();

    aName.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aName);
    aName.IsChange.Should().BeTrue();
    aName.OldValue.Should().Be(TestName);
    aName.NewValue.Should().Be(TestNameUpdate);

    aCreated.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aCreated);
    aCreated.IsChange.Should().BeFalse();
    if (typeof(TPK) == typeof(ObjectId))
      DatabaseCRUDHelper.CompareMongoDateTime((DateTime)(aCreated.OldValue ?? throw new InvalidCastException()), TestDateTime).Should().Be(0);
    else
      aCreated.OldValue.Should().Be(TestDateTime);
    aCreated.NewValue.Should().BeNull();
    aCreated.DataType.ToLower().Should().NotContain("string");

    aNullValue.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aNullValue);
    aNullValue.IsChange.Should().BeTrue();
    aNullValue.OldValue.Should().Be(TestName);
    aNullValue.NewValue.Should().BeNull();

    aNullValue2.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aNullValue2);
    aNullValue2.IsChange.Should().BeTrue();
    aNullValue2.OldValue.Should().BeNull();
    aNullValue2.NewValue.Should().Be(TestNameUpdate);

    aNullValue3.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aNullValue3);
    aNullValue3.IsChange.Should().BeFalse();
    aNullValue3.OldValue.Should().BeNull();
    aNullValue3.NewValue.Should().BeNull();
  }

  public static async Task UpdateItemWithoutChangesAsyncTest<TPK>(IMediator mediator, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    var testAuditEntityType = typeof(TPK) == typeof(ObjectId) ? TestAuditMongoEntityName : TestAuditEntityName;
    // Action.
    var item = new Fake1AuditData<TPK>
    {
      Created = TestDateTime,
      Name = TestName,
      NullValue = TestName,
      NullValue2 = null,
      NotAuditableColumn = "Audit"
    };

    // Act.
    var result = await mediator.Send(new Fake1AuditSaveCommand<TPK>(item));

    var allData = (await mediator.Send(new Fake1AuditGetQuery<TPK>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<Fake1AuditData<TPK>, TPK>(result, allData);

    // Update
    item.Id = itemId;
    await mediator.Send(new Fake1AuditSaveCommand<TPK>(item));


    // Assert.
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TPK>(getTableName(testAuditEntityType), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(2);
    resAuditItems.Last().State.Should().Be(AuditInfoStateEnum.Modified);

    var auditItem = resAuditItems.Single(a => a.State == AuditInfoStateEnum.Modified);
    // only Name was changed
    CheckCount(auditItem);
    auditItem.Columns.All(c => c.IsChange).Should().Be(false);
  }

  public static async Task DeleteItemTest<TPK>(IMediator mediator, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    var testAuditEntityType = typeof(TPK) == typeof(ObjectId) ? TestAuditMongoEntityName : TestAuditEntityName;

    // Arrange.
    var item = new Fake1AuditData<TPK>
    {
      Created = TestDateTime,
      Name = TestName,
      NotAuditableColumn = "Audit",
      NullValue = TestName
    };

    var result = await mediator.Send(new Fake1AuditSaveCommand<TPK>(item));
    var allData = (await mediator.Send(new Fake1AuditGetQuery<TPK>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<Fake1AuditData<TPK>, TPK>(result, allData);

    // Action.
    var resultDelete = await mediator.Send(new Fake1AuditDeleteCommand<TPK>(itemId));

    // Assert.
    resultDelete.IsSuccess.Should().BeTrue();

    var resAuditItems = (await mediator.Send(new AuditGetQuery<TPK>(getTableName(testAuditEntityType), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems);
    resAuditItems.Should().HaveCount(2);
    resAuditItems.Last().State.Should().Be(AuditInfoStateEnum.Deleted);


    var auditItem = resAuditItems.Last();
    CheckCount(auditItem);
    auditItem.Columns.All(c => c.IsChange).Should().Be(true);

    var aid = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.Id)));
    var aName = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.Name)));
    var aCreated = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.Created)));
    var aNullValue = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.NullValue)));
    var aNullValue2 = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.NullValue2)));
    var aNullValue3 = auditItem.GetColumn(getColumnName(testAuditEntityType, nameof(Fake1AuditEntity.NullValue3)));

    aid.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aid);
    aid.OldValue.Should().Be(itemId);
    aid.NewValue.Should().BeNull();
    aid.DataType.ToLower().Should().NotContain("string");

    aName.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aName);
    aName.OldValue.Should().Be(TestName);
    aName.NewValue.Should().BeNull();

    aCreated.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aCreated);
    if (typeof(TPK) == typeof(ObjectId))
      DatabaseCRUDHelper.CompareMongoDateTime((DateTime)(aCreated.OldValue ?? throw new InvalidCastException()), TestDateTime).Should().Be(0);
    else
      aCreated.OldValue.Should().Be(TestDateTime);
    aCreated.NewValue.Should().BeNull();
    aCreated.DataType.ToLower().Should().NotContain("string");

    aNullValue.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aNullValue);
    aNullValue.OldValue.Should().Be(TestName);
    aNullValue.NewValue.Should().BeNull();

    aNullValue2.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aNullValue2);
    aNullValue2.OldValue.Should().BeNull();
    aNullValue2.NewValue.Should().BeNull();

    aNullValue3.Should().NotBeNull();
    ArgumentNullException.ThrowIfNull(aNullValue3);
    aNullValue3.OldValue.Should().BeNull();
    aNullValue3.NewValue.Should().BeNull();
  }

  private static void CheckCount<TPK>(AuditGetDataOut<TPK> auditItem)
  {
    auditItem.Columns.Where(c => c.PropName != nameof(PKMongoEntity.Version)).Should().HaveCount(6);
  }
}