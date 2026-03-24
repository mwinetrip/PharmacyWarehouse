namespace PharmacyWarehouse.Models;

// Приходная накладная
public class IncomingInvoice : Document
{
    public Supplier Supplier { get; set; } // Поставщик
}