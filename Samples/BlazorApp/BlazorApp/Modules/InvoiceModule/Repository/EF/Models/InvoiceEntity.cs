using ACore.Server.Modules.AuditModule.Attributes;
using ACore.Server.Repository.Contexts.EF.Models.PK;

// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace BlazorApp.Modules.InvoiceModule.Repository.EF.Models;

[Auditable(1)]

internal class InvoiceEntity : PKIntEntity
{
  public int CustomerId { get; set; }
  public DateTime InvoiceDate { get; set; }

  public DateTime DueDate { get; set; }

  public int StatusId { get; set; }

  public decimal TotalAmount { get; set; }

  public ICollection<InvoiceItemEntity>? InvoiceItems { get; set; }
}