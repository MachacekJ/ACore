using ACore.Server.Repository.Contexts.EF.Models;

namespace BlazorApp.Modules.InvoiceModule.Repository.EF.PG.Scripts;

// ReSharper disable once InconsistentNaming
internal class V1_0_0_2SeedStatus : EFVersionScriptsBase
{
    public override Version Version => new ("1.0.0.2");

    public override List<string> AllScripts
    {
        get
        {

            List<string> l = new()
            {
                @"
INSERT INTO invoice.invoice_status (""invoice_status_id"", ""name"") VALUES(1,'Created');
INSERT INTO invoice.invoice_status (""invoice_status_id"",""name"") VALUES(2,'Sent');
INSERT INTO invoice.invoice_status (""invoice_status_id"",""name"") VALUES(3,'Paid');
INSERT INTO invoice.invoice_status (""invoice_status_id"",""name"") VALUES(4,'Cancelled');
"
            };


            return l;
        }
    }
}