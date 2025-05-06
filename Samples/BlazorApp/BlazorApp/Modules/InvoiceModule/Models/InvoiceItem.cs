using BlazorApp.Modules.InvoiceModule.Repository.EF.Models;
using Mapster;

namespace BlazorApp.Modules.InvoiceModule.Models;

public class InvoiceItem
{
  public int CustomerId { get; set; }
  public DateTime InvoiceDate { get; set; }

  public DateTime DueDate { get; set; }

  public int StatusId { get; set; }

  public decimal TotalAmount { get; set; }

  public ICollection<InvoiceItemItem>? InvoiceItems { get; set; }
}

internal static class InvoiceItemExtensions
{
  internal static InvoiceItem ToItem(this InvoiceEntity invoiceEntity)
  {
    var inv = invoiceEntity.Adapt<InvoiceItem>();
    if (invoiceEntity.InvoiceItems == null || invoiceEntity.InvoiceItems.Count == 0)
      return inv;
    inv.InvoiceItems = new List<InvoiceItemItem>();
    foreach (var invoiceEntityInvoiceItem in invoiceEntity.InvoiceItems)
    {
      inv.InvoiceItems.Add(invoiceEntityInvoiceItem.ToItem());
    }

    return inv;
  }

  internal static InvoiceEntity ToEntity(this InvoiceItem invoiceItem)
  {
    var ent = invoiceItem.Adapt<InvoiceEntity>();
    if (invoiceItem.InvoiceItems == null || invoiceItem.InvoiceItems.Count == 0)
      return ent;
    ent.InvoiceItems = new List<InvoiceItemEntity>();
    foreach (var item in invoiceItem.InvoiceItems)
    {
      ent.InvoiceItems.Add(item.ToEntity());
    }

    return ent;
  }
}