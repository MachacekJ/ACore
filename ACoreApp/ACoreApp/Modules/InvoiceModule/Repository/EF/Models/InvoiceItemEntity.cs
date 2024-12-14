using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Repository.Contexts.EF.Models.PK;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ACoreApp.Modules.InvoiceModule.Repository.EF.Models;

[Auditable(1)]
internal class InvoiceItemEntity : PKLongEntity
{
  public int InvoiceId { get; set; }

  [MaxLength(500)]
  public string? Description { get; set; }

  public decimal UnitPrice { get; set; }
  public int Quantity { get; set; }
  public decimal TotalPrice { get; set; }

  [ForeignKey(nameof(InvoiceId))]
  public InvoiceEntity? Invoice { get; set; }
}