using ACore.Extensions;
using ACore.Tests.Server.TestImplementations.Modules.TestModule.Repositories.SQL.Models;
using Mapster;
using MongoDB.Bson;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.CQRS.TestNoAudit.Models;

public class TestNoAuditData<TPK>(string name)
{
  public TPK Id { get; set; } = default(TPK) ?? throw new Exception($"Cannot create {nameof(Id)} for type {typeof(TPK).Name}");

  public string Name { get; set; } = name;

  public DateTime Created { get; set; }

  internal static KeyValuePair<string, TestNoAuditData<T>> Create<T>(TestNoAuditEntity noAuditEntity, string saltSumHash)
  {
    var testPKGuidData = noAuditEntity.Adapt<TestNoAuditData<T>>();
    return new KeyValuePair<string, TestNoAuditData<T>>(noAuditEntity.GetSumHash(saltSumHash), testPKGuidData);
  }

  internal static KeyValuePair<string, TestNoAuditData<T>> Create<T>(Repositories.Mongo.Models.TestNoAuditEntity noAuditEntity, string saltSumHash)
  {
    var testPKGuidData = noAuditEntity.Adapt<TestNoAuditData<T>>();
    return new KeyValuePair<string, TestNoAuditData<T>>(noAuditEntity.GetSumHash(saltSumHash), testPKGuidData);
  }
}
public static class TestNoAuditData
{
  public static void MapConfig()
  {
    TypeAdapterConfig<TestNoAuditEntity, TestNoAuditData<int>>.NewConfig()
      .ConstructUsing(src => new TestNoAuditData<int>(src.Name));
    TypeAdapterConfig<Repositories.Mongo.Models.TestNoAuditEntity, TestNoAuditData<ObjectId>>.NewConfig()
      .ConstructUsing(src => new TestNoAuditData<ObjectId>(src.Name));
  }
}