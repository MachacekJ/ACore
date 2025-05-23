﻿using System.Reflection;
using ACore.Models.Cache;
using ACore.Server.Modules.SettingsDbModule.Repositories;
using ACore.Server.Modules.SettingsDbModule.Repositories.EF.Models;
using ACore.Server.Repository;
using FluentAssertions;
using MediatR;
using Xunit;

namespace ACore.Tests.Server.Tests.Modules.SettingsDbModule.Repositories;

public class SettingsDbModuleSqlMemoryStorageImplTests : SettingsDbModuleTestsBase
{
  [Fact]
  public async Task SaveGetTest()
  {
    var method = MethodBase.GetCurrentMethod();
    await RunTestAsync(method, async () =>
    {
      await CheckSettingEntity( MemorySettingStorageModule ?? throw new ArgumentNullException($"{nameof(MemorySettingStorageModule)} is null."), Mediator);
    });
  }
  
#pragma warning disable xUnit1013
  private static async Task CheckSettingEntity(ISettingsDbModuleRepository dbModule, IMediator mediator)
#pragma warning restore xUnit1013
  {
    string key = "key";
    string key2 = "key2";
    string value = "value";
    string value2 = "value2";
    await dbModule.Setting_SaveAsync(key, value, true);
    var val = await dbModule.Setting_GetAsync(key);
    val.Should().Be(value);

    await dbModule.Setting_SaveAsync(key, value2, true);
    var val2 = await dbModule.Setting_GetAsync(key);
    val2.Should().Be(value2);

    // Check if is value in cache
    var keyCache = CacheKey.Create(CacheCategories.Entity, nameof(SettingsEntity));
    // var cacheValue = await mediator.Send(new MemoryCacheModuleGetQuery(keyCache));
    // var mem = cacheValue.ResultValue?.ObjectValue as List<SettingsEntity>;
    // Assert.True(mem != null && mem.First(a => a.Key == key).Value == value2);

    Exception? isError = null;
    try
    {
      await dbModule.Setting_GetAsync(key2);
    }
    catch (Exception ex)
    {
      isError = ex;
    }

    Assert.True(isError != null);
  }
}