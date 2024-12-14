namespace ACoreApp.Modules.InvoiceModule.CQRS.InvoiceGet.Models;

public record InvoiceGetDataOut
{
  public static InvoiceGetDataOut Create()
  {
    return new InvoiceGetDataOut();
  }
}