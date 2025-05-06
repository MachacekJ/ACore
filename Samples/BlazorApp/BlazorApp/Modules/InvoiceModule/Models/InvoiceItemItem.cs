using BlazorApp.Modules.InvoiceModule.Repository.EF.Models;
using Mapster;

namespace BlazorApp.Modules.InvoiceModule.Models;

public class InvoiceItemItem()
{
  public int InvoiceId { get; set; }

  public string? Description { get; set; }

  public decimal UnitPrice { get; set; }
  public int Quantity { get; set; }
  public decimal TotalPrice { get; set; }

  public InvoiceItem? Invoice { get; set; }
}

internal static class InvoiceItemItemExtensions
{
  internal static InvoiceItemItem ToItem(this InvoiceItemEntity invoiceItemEntity) 
    => invoiceItemEntity.Adapt<InvoiceItemItem>();

  internal static InvoiceItemEntity ToEntity(this InvoiceItemItem invoiceItemItem) 
    => invoiceItemItem.Adapt<InvoiceItemEntity>();
}