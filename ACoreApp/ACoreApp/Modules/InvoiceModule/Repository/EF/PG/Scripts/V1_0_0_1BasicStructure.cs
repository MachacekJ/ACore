using ACore.Server.Repository.Contexts.EF.Models;

namespace ACoreApp.Modules.InvoiceModule.Repository.EF.PG.Scripts;

// ReSharper disable once InconsistentNaming
internal class V1_0_0_1BasicStructure : EFVersionScriptsBase
{
    public override Version Version => new ("1.0.0.1");

    public override List<string> AllScripts
    {
        get
        {

            List<string> l =
            [
                @"
CREATE SCHEMA invoice
    AUTHORIZATION ""user"";


CREATE table invoice.invoice_status (
  invoice_status_id SMALLINT PRIMARY KEY,
  name VARCHAR(100)      
);

CREATE TABLE invoice.invoice (
    invoice_id INT GENERATED ALWAYS AS IDENTITY
                PRIMARY KEY,  
    customer_id UUID NOT null,
    invoice_date DATE NOT NULL,  
    due_date DATE NOT NULL,  
    total_amount NUMERIC(10,2) NOT null,  
    invoice_status_id SMALLINT NOT null
        CONSTRAINT fk_invoice_status
          REFERENCES invoice.invoice_status(invoice_status_id)
);

CREATE TABLE invoice.invoice_item (
    invoice_item_id BIGINT GENERATED ALWAYS AS IDENTITY,  
    invoice_id INT NOT null 
     CONSTRAINT fk_invoice 
       REFERENCES invoice.invoice(invoice_id) ON DELETE CASCADE,  
    description TEXT,  
    quantity INT CHECK (quantity > 0) NOT NULL,  
    unit_price DECIMAL(10,2) NOT NULL,  
    total_price DECIMAL(10,2) GENERATED ALWAYS AS (quantity * unit_price) STORED
);
"
            ];


            return l;
        }
    }
}