using System.ComponentModel.DataAnnotations;
using ACore.Server.Storages.Contexts.EF.Models.PK;
using Microsoft.EntityFrameworkCore;

namespace ACore.Tests.Server.TestImplementations.Modules.TestModule.Storages.Models;

public class TestValueTypeEntityBase<TPK>(TPK id) : PKEntity<TPK>(id)
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
}