using System.Linq.Expressions;
using ACore.Server.Repository.Contexts.EF.Models;
using ACoreApp.Modules.InvoiceModule.Repository.EF.Models;

#pragma warning disable CS8603 // Possible null reference return.

namespace ACoreApp.Modules.InvoiceModule.Repository.EF.PG;

internal static class DefaultNames
{
  private const string SchemaName = "invoice";

  public static Dictionary<string, EFInternalName> ObjectNameMapping => new()
  {
    { nameof(InvoiceEntity), new EFInternalName("invoice", InvoiceColumnEntityColumnNames, SchemaName) },
    { nameof(InvoiceItemEntity), new EFInternalName("invoice_item", InvoiceItemEntityColumnNames, SchemaName) },
  };

  private static Dictionary<Expression<Func<InvoiceEntity, object>>, string> InvoiceColumnEntityColumnNames => new()
  {
    { e => e.Id, "invoice_id" },
    { e => e.CustomerId, "customer_id" },
    { e => e.InvoiceDate, "invoice_date" },
    { e => e.DueDate, "due_date" },
    { e => e.TotalAmount, "total_amount" },
    { e => e.StatusId, "invoice_status_id" },
  };

  private static Dictionary<Expression<Func<InvoiceItemEntity, object>>, string> InvoiceItemEntityColumnNames => new()
  {
    { e => e.Id, "invoice_item_id" },
    { e => e.InvoiceId, "invoice_id" },
    { e => e.Description, "description" },
    { e => e.UnitPrice, "unit_price" },
    { e => e.Quantity, "quantity" },
    { e => e.TotalPrice, "total_price" },
  };
}