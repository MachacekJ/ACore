﻿using System.Reflection;
using Xunit;

namespace ACore.TestsIntegrations.Modules.SettingModule;

public class Settings : BasicStructureBase
{
  [Fact]
  public async Task SaveGetTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunStorageTestAsync(StorageTypesToTest, method, async (storageType) =>
    {
      var db = GetBasicStorageModule(storageType);
      await Tests.Server.Modules.SettingModule.MemorySettingStorageModule.CheckSettingEntity(db, Mediator); 
    });
  }
}