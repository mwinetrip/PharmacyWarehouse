namespace PharmacyWarehouse.Models;

// Поставщик
public class Supplier : Person
{
    public string Bank { get; set; } // Название Банка
    public string AccountNumber { get; set; } // Номер счёта
    
    // Информация о поставщике
    public override string GetInfo()
    {
        return $"{base.GetInfo()}, Банк: {{Bank}}, Счет: {{AccountNumber}}";
    }
}