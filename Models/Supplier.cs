namespace PharmacyWarehouse.Models;

// Класс: Поставщик
public class Supplier : Person
{
    public string Bank { get; set; } = string.Empty;           // Название банка
    public string AccountNumber { get; set; } = string.Empty;  // Расчётный счёт

    public override string GetInfo() =>
        $"{base.GetInfo()}, банк: {Bank}, счёт: {AccountNumber}";
}