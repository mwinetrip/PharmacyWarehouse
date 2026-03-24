namespace PharmacyWarehouse.Models;

// Счёт-фактура
public class SalesInvoice : Document
{
    public Customer Customer { get; set; } // Покупатель
    public string SellerName { get; set; } // Фамилия продавца
}