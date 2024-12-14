using ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.SQL.Models;
using Mapster;

namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Models;

public class Fake1ValueTypeData<T> //: HashData
{
  public T Id { get; set; } = default(T) ?? throw new Exception($"Cannot create {nameof(Id)} for type {typeof(T).Name}");
  public int IntNotNull { get; set; }
  public int? IntNull { get; set; }
  public long BigIntNotNull { get; set; }
  public long? BigIntNull { get; set; }
  public bool Bit2 { get; set; }
  public string? Char2 { get; set; }
  public DateTime Date2 { get; set; }
  public DateTime DateTime2 { get; set; }
  public decimal Decimal2 { get; set; }
  public string NChar2 { get; set; } = string.Empty;
  public string NVarChar2 { get; set; } = string.Empty;
  public DateTime SmallDateTime2 { get; set; }
  public short SmallInt2 { get; set; }
  public byte TinyInt2 { get; set; }
  public Guid Guid2 { get; set; }
  public byte[] VarBinary2 { get; set; } = [];
  public string VarChar2 { get; set; } = string.Empty;

  public TimeSpan? TimeSpan2 { get; set; }

  internal static Fake1ValueTypeData<TPK> Create<TPK>(Fake1ValueTypeEntity entity)
  {
#pragma warning disable CS8603 // Possible null reference return.
    var config = TypeAdapterConfig<Fake1ValueTypeEntity, Fake1ValueTypeData<TPK>>.NewConfig()
      .Ignore(d => d.TimeSpan2).Config;
#pragma warning restore CS8603 // Possible null reference return.
  
    var res = entity.Adapt<Fake1ValueTypeData<TPK>>(config);
    res.TimeSpan2 = entity.TimeSpan2 != null ? new TimeSpan(entity.TimeSpan2.Value) : null;
    return res;
  }

  internal static Fake1ValueTypeData<TPK> Create<TPK>(Repositories.Mongo.Models.Fake1ValueTypeEntity entity)
  {
#pragma warning disable CS8603 // Possible null reference return.
    var config = TypeAdapterConfig<Repositories.Mongo.Models.Fake1ValueTypeEntity, Fake1ValueTypeData<TPK>>.NewConfig()
      .Ignore(d => d.TimeSpan2).Config;
#pragma warning restore CS8603 // Possible null reference return.
    
    var res = entity.Adapt<Fake1ValueTypeData<TPK>>(config);
    res.TimeSpan2 = entity.TimeSpan2 != null ? new TimeSpan(entity.TimeSpan2.Value) : null;
    return res;
  }
}