﻿using System.Reflection;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Get;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Models;
using ACore.AppTest.Modules.TestModule.CQRS.TestAttributeAudit.Save;
using ACore.AppTest.Modules.TestModule.Storages.Mongo.Models;
using ACore.Server.Modules.AuditModule.CQRS.Audit.AuditGet;
using ACore.Server.Modules.AuditModule.CQRS.Audit.AuditGet.Models;
using ACore.Server.Modules.AuditModule.Models;
using ACore.Tests.Server.Modules.TestModule;
using FluentAssertions;
using MongoDB.Bson;
using Xunit;

namespace ACore.TestsIntegrations.Modules.TestModule.Mongo;

public class AuditAttributeTests : MongoAuditBase
{
  [Fact]
  public async Task AddItemTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var testDateTime = DateTime.UtcNow;
      var testName = "AuditTest";
      var entityName = nameof(TestAttributeAuditPKMongoEntity);

      // Action.
      var item = new TestAttributeAuditData<ObjectId>
      {
        Created = testDateTime,
        Name = testName,
        NotAuditableColumn = "Audit"
      };

      await Mediator.Send(new TestAttributeAuditSaveCommand<ObjectId>(item));
      var itemId = item.Id;
      itemId.Should().NotBeNull();
      itemId.Should().NotBe(ObjectId.Empty);

      // Assert.
      var allData = (await Mediator.Send(new TestAttributeAuditGetQuery<ObjectId>())).ResultValue;
      allData.Should().HaveCount(1);

      var savedItem = allData.Single();
      var resAuditItems = (await Mediator.Send(new AuditGetQuery<ObjectId>(GetTestTableName(storageType, entityName), savedItem.Id))).ResultValue;
      resAuditItems.Should().HaveCount(1);
      resAuditItems.Single().EntityState.Should().Be(AuditStateEnum.Added);

      var auditItem = resAuditItems.Single();
      auditItem.Columns.Should().HaveCount(3);

      var aid = auditItem.GetColumn(GetTestColumnName(storageType, entityName, nameof(TestAttributeAuditData<ObjectId>.Id)));
      var aName = auditItem.GetColumn(GetTestColumnName(storageType, entityName, nameof(TestAttributeAuditData<ObjectId>.Name)));
      var aCreated = auditItem.GetColumn(GetTestColumnName(storageType, entityName, nameof(TestAttributeAuditData<ObjectId>.Created)));

      aid.Should().NotBeNull();
      aName.Should().NotBeNull();
      // ReSharper disable once NullableWarningSuppressionIsUsed
      aName!.NewValue.Should().NotBeNull();
      aCreated.Should().NotBeNull();
      // ReSharper disable once NullableWarningSuppressionIsUsed
      aCreated!.NewValue.Should().NotBeNull();

      // ReSharper disable once NullableWarningSuppressionIsUsed
      aid!.NewValue.Should().Be(savedItem.Id);
      aName.NewValue.Should().Be(testName);
      var aa = Convert.ToDateTime(aCreated.NewValue);
      aa.Should().Be(testDateTime);
    });
  }

  [Fact]
  public async Task UpdateItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) => { await AuditAttributeTHelper.UpdateItemAsyncTest(Mediator, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop)); });
  }

  [Fact]
  public async Task DeleteItem()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) => { await AuditAttributeTHelper.DeleteItemTest(Mediator, (name) => GetTestTableName(storageType, name), (name, prop) => GetTestColumnName(storageType, name, prop)); });
  }
}