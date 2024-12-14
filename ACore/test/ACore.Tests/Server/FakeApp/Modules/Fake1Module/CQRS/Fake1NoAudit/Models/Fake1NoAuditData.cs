using ACore.Extensions;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using Mapster;
using MongoDB.Bson;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1NoAudit.Models;

public class Fake1NoAuditData<TPK>(string name)
{
  public TPK Id { get; set; } = default(TPK) ?? throw new Exception($"Cannot create {nameof(Id)} for type {typeof(TPK).Name}");

  public string Name { get; set; } = name;

  public DateTime Created { get; set; }

  internal static KeyValuePair<string, Fake1NoAuditData<T>> Create<T>(Fake1NoAuditEntity noAuditEntity, string saltSumHash)
  {
    var testPKGuidData = noAuditEntity.Adapt<Fake1NoAuditData<T>>();
    return new KeyValuePair<string, Fake1NoAuditData<T>>(noAuditEntity.GetSumHash(saltSumHash), testPKGuidData);
  }

  internal static KeyValuePair<string, Fake1NoAuditData<T>> Create<T>(Repositories.Mongo.Models.Fake1NoAuditEntity noAuditEntity, string saltSumHash)
  {
    var testPKGuidData = noAuditEntity.Adapt<Fake1NoAuditData<T>>();
    return new KeyValuePair<string, Fake1NoAuditData<T>>(noAuditEntity.GetSumHash(saltSumHash), testPKGuidData);
  }
}
public static class TestNoAuditData
{
  public static void MapsterConfig()
  {
    TypeAdapterConfig<Fake1NoAuditEntity, Fake1NoAuditData<int>>.NewConfig()
      .ConstructUsing(src => new Fake1NoAuditData<int>(src.Name));
    TypeAdapterConfig<Repositories.Mongo.Models.Fake1NoAuditEntity, Fake1NoAuditData<ObjectId>>.NewConfig()
      .ConstructUsing(src => new Fake1NoAuditData<ObjectId>(src.Name));
  }
}