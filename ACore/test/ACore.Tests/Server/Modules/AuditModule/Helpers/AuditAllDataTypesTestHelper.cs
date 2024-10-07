using System.Text.Json;
using ACore.Server.Modules.AuditModule.CQRS.AuditGet;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Get;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Models;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.CQRS.TestValueType.Save;
using ACore.Tests.Server.TestImplementations.Server.Modules.TestModule.Storages.Mongo.Models;
using FluentAssertions;
using MediatR;
using Serilog.Sinks.InMemory;

namespace ACore.Tests.Server.Modules.AuditModule.Helpers;

public static class AuditAllDataTypesTestHelper
{
  public static async Task AllDataTypes<TPK>(IMediator mediator, InMemorySink logInMemorySink, Func<string, string> getTableName, Func<string, string, string> getColumnName)
  {
    var entityName = "TestValueTypeEntity";

    // Arrange
    var item = new TestValueTypeData<TPK>
    {
      IntNotNull = int.MaxValue,
      IntNull = int.MaxValue,
      BigIntNotNull = long.MaxValue,
      BigIntNull = long.MaxValue,
      Bit2 = true,
      Char2 = "Hello",
      Date2 = DateTime.Today.ToUniversalTime(),
      DateTime2 = DateTime.UtcNow,
      Decimal2 = 12345678901.12345678M,
      NChar2 = "Čau říá",
      NVarChar2 = "říkám já",
      SmallDateTime2 = new DateTime(2000, 10, 10, 10, 10, 0, DateTimeKind.Utc),
      SmallInt2 = short.MaxValue,
      TinyInt2 = byte.MaxValue,
      Guid2 = Guid.NewGuid(),
      VarBinary2 = new byte[10001],
      VarChar2 = "říkám já řřČŘÉÍÁ"
    };

    // Act.
    var result = await mediator.Send(new TestValueTypeSaveCommand<TPK>(item));
    // Assert

    var allData = (await mediator.Send(new TestValueTypeGetQuery<TPK>())).ResultValue;
    var itemId = AuditAssertTestHelper.AssertSinglePrimaryKeyWithResult<TestValueTypeData<TPK>, TPK>(result, allData);
    var resAuditItems = (await mediator.Send(new AuditGetQuery<TPK>(getTableName(entityName), itemId))).ResultValue;
 
    
    ArgumentNullException.ThrowIfNull(allData);
    allData.Should().HaveCount(1);

    var savedItem = allData.Single();
    ArgumentNullException.ThrowIfNull(resAuditItems);

    var auditItem = resAuditItems.Single();
    // 17 fields + 1 Id
    auditItem.Columns.Should().HaveCount(18);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.Id))).NewValue.Should().Be(itemId);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.IntNotNull))).NewValue.Should().Be(item.IntNotNull);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.IntNull))).NewValue.Should().Be(item.IntNull);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.BigIntNotNull))).NewValue.Should().Be(item.BigIntNotNull);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.BigIntNull))).NewValue.Should().Be(item.BigIntNull);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.Bit2))).NewValue.Should().Be(item.Bit2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.Char2))).NewValue.Should().Be(item.Char2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.Date2))).NewValue.Should().Be(item.Date2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.DateTime2))).NewValue.Should().Be(item.DateTime2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.Decimal2))).NewValue.Should().Be(item.Decimal2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.NChar2))).NewValue.Should().Be(item.NChar2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.NVarChar2))).NewValue.Should().Be(item.NVarChar2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.SmallDateTime2))).NewValue.Should().Be(item.SmallDateTime2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.SmallInt2))).NewValue.Should().Be(item.SmallInt2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.TinyInt2))).NewValue.Should().Be(item.TinyInt2);
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.Guid2))).NewValue.Should().Be(item.Guid2);
    var newVal = auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.VarBinary2))).NewValue;
    JsonSerializer.Serialize(newVal).Should().Be(JsonSerializer.Serialize(item.VarBinary2));
    auditItem.Columns.Single(a => a.ColumnName == getColumnName(entityName, nameof(TestValueTypeEntity.VarChar2))).NewValue.Should().Be(item.VarChar2);
  }
}