using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestValueType.Models;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Models;
using Mapster;
using MongoDB.Bson;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.Mongo.Models;

[Auditable(1)]
internal class TestValueTypeEntity() : TestValueTypeEntityBase<ObjectId>(PKMongoEntity.EmptyId)
{
  public static TestValueTypeEntity Create<TPK>(TestValueTypeData<TPK> data)
  {
#pragma warning disable CS8603 // Possible null reference return.
    var config = TypeAdapterConfig<TestValueTypeData<TPK>, TestValueTypeEntity>.NewConfig()
      .Ignore(d => d.TimeSpan2).Config;
#pragma warning restore CS8603 // Possible null reference return.
 
    var res = ToEntity<TestValueTypeEntity>(data, config);
    res.TimeSpan2 = data.TimeSpan2?.Ticks;
    return res;
  }
}