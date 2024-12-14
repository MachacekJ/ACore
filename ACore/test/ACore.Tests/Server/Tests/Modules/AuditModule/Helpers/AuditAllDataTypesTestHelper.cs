using System.Text.Json;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Server.Repository.Contexts.Mongo.Models.PK;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Get;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Models;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Save;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;
using FluentAssertions;
using MediatR;
using Serilog.Sinks.InMemory;

namespace ACore.Tests.Server.Tests.Modules.AuditModule.Helpers;

public static class AuditAllDataTypesTestHelper
{
  public static async Task AllDataTypes<TPK>(IMediator mediator, InMemorySink logInMemorySink, bool isMongoRounded, Func<Type, string> getTableName, Func<Type, string, string> getColumnName)
  {
    var entityName = typeof(Fake1ValueTypeEntity);
    // in Mongo is datetime ISO date and its tick 638819641158319999 rounded to 638819641158310000
    var dt2 = new DateTime(638819641158319999).ToUniversalTime();
    // Arrange
    var item = new Fake1ValueTypeData<TPK>
    {
      IntNotNull = int.MaxValue,
      IntNull = int.MaxValue,
      BigIntNotNull = long.MaxValue,
      BigIntNull = long.MaxValue,
      Bit2 = true,
      Char2 = "Hello",
      Date2 = dt2,
      DateTime2 = dt2,
      Decimal2 = 12345678901.12345678M,
      NChar2 = "Čau říá",
      NVarChar2 = "říkám já",
      SmallDateTime2 = new DateTime(2000, 10, 10, 10, 10, 0, DateTimeKind.Utc),
      SmallInt2 = short.MaxValue,
      TinyInt2 = byte.MaxValue,
      Guid2 = Guid.NewGuid(),
      VarBinary2 = new byte[10001],
      VarChar2 = "říkám já řřČŘÉÍÁ",
      TimeSpan2 = TimeSpan.MaxValue
    };

    #region Insert

    // Act.
    var result = await mediator.Send(new Fake1ValueTypeSaveHashCommand<TPK>(item));

    // Assert 1
    var allData = (await mediator.Send(new Fake1ValueTypeGetQuery<TPK>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<Fake1ValueTypeData<TPK>, TPK>(result, allData);
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TPK>(getTableName(entityName), itemId))).ResultValue;

    ArgumentNullException.ThrowIfNull(allData);
    allData.Should().HaveCount(1);
    
    ArgumentNullException.ThrowIfNull(resAuditItems);

    var auditItem = resAuditItems.Single();
    // 17 fields + 1 Id
    auditItem.Columns.Where(c => c.PropName != nameof(PKMongoEntity.Version)).Should().HaveCount(19);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.Id))).NewValue.Should().Be(itemId);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.IntNotNull))).NewValue.Should().Be(item.IntNotNull);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.IntNull))).NewValue.Should().Be(item.IntNull);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.BigIntNotNull))).NewValue.Should().Be(item.BigIntNotNull);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.BigIntNull))).NewValue.Should().Be(item.BigIntNull);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.Bit2))).NewValue.Should().Be(item.Bit2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.Char2))).NewValue.Should().Be(item.Char2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.Date2))).NewValue.Should().Be(item.Date2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.DateTime2))).NewValue.Should().Be(item.DateTime2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.Decimal2))).NewValue.Should().Be(item.Decimal2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.NChar2))).NewValue.Should().Be(item.NChar2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.NVarChar2))).NewValue.Should().Be(item.NVarChar2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.SmallDateTime2))).NewValue.Should().Be(item.SmallDateTime2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.SmallInt2))).NewValue.Should().Be(item.SmallInt2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.TinyInt2))).NewValue.Should().Be(item.TinyInt2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.Guid2))).NewValue.Should().Be(item.Guid2);
    var newVal = auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.VarBinary2))).NewValue;
    JsonSerializer.Serialize(newVal).Should().Be(JsonSerializer.Serialize(item.VarBinary2));
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.VarChar2))).NewValue.Should().Be(item.VarChar2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(Fake1ValueTypeEntity.TimeSpan2))).NewValue.Should().Be(item.TimeSpan2.Value.Ticks);

    #endregion

    #region Update without change

    // Act2 Arrange Update - without audit - no change
    item.Id = itemId;

    if (isMongoRounded)
    {
      // in Mongo is ISO date 638819641158319999 rounded to 638819641158310000
      item.DateTime2 = new DateTime(638819641158318888).ToUniversalTime();
      item.Date2 = new DateTime(638819641158318888).ToUniversalTime();
    }

    // Act2 Act.
    var updateResult = await mediator.Send(new Fake1ValueTypeSaveHashCommand<TPK>(item));
    updateResult.IsSuccess.Should().BeTrue();

    // Act2 Assert
    var resAuditItems2 = (await mediator.Send(new AuditGetQuery<TPK>(getTableName(entityName), itemId))).ResultValue;
    resAuditItems2.Should().HaveCount(2);
    var allLastCol = resAuditItems2.OrderBy(a => a.Created).Last();
    var test = allLastCol.Columns.Where(ab => ab.IsChange).ToList();
    test.Any().Should().BeFalse();

    #endregion

    #region Update with change

    // Act2 Update - without audit - no change
    item.TimeSpan2 = new TimeSpan(item.TimeSpan2.Value.Ticks - 1);
    await mediator.Send(new Fake1ValueTypeSaveHashCommand<TPK>(item));
    var resAuditItems3 = (await mediator.Send(new AuditGetQuery<TPK>(getTableName(entityName), itemId))).ResultValue;
    ArgumentNullException.ThrowIfNull(resAuditItems3);
    resAuditItems3.Should().HaveCount(3);
    resAuditItems3.Where(e => e.State == AuditInfoStateEnum.Modified).Should().HaveCount(2);

    #endregion
  }
}