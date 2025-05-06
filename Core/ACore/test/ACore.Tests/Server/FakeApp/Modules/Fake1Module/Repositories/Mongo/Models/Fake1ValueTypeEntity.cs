using System.ComponentModel.DataAnnotations;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Repository.Attributes;
using ACore.Server.Repository.Contexts.Mongo.Models.PK;
using ACore.Tests.Server.FakeApp.Modules.Fake1Module.CQRS.Fake1ValueType.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace ACore.Tests.Server.FakeApp.Modules.Fake1Module.Repositories.Mongo.Models;

[Auditable(1)]
[MongoCollectionName("fake1_value_types")]
internal class Fake1ValueTypeEntity : PKMongoEntity
{
  
  public int IntNotNull { get; set; }
  public int? IntNull { get; set; }
  public long BigIntNotNull { get; set; }
  public long? BigIntNull { get; set; }
  public bool Bit2 { get; set; }

  [MaxLength(10)]
  public string? Char2 { get; set; }

  public DateTime Date2 { get; set; }
  public DateTime DateTime2 { get; set; }

  [Precision(19, 8)]
  public decimal Decimal2 { get; set; }

  [MaxLength(10)]
  public string NChar2 { get; set; } = string.Empty;

  [MaxLength(10)]
  public string NVarChar2 { get; set; } = string.Empty;

  public DateTime SmallDateTime2 { get; set; }
  public short SmallInt2 { get; set; }
  public byte TinyInt2 { get; set; }
  public Guid Guid2 { get; set; }
  public byte[] VarBinary2 { get; set; } = [];

  [MaxLength(100)]
  public string VarChar2 { get; set; } = string.Empty;

  public long? TimeSpan2 { get; set; }
  
  public static Fake1ValueTypeEntity Create<TPK>(Fake1ValueTypeData<TPK> data)
  {
#pragma warning disable CS8603 // Possible null reference return.
    var config = TypeAdapterConfig<Fake1ValueTypeData<TPK>, Fake1ValueTypeEntity>.NewConfig()
      .Ignore(d => d.TimeSpan2).Config;
#pragma warning restore CS8603 // Possible null reference return.
 
    var res = ToEntity<Fake1ValueTypeEntity>(data, config);
    res.TimeSpan2 = data.TimeSpan2?.Ticks;
    return res;
  }
}