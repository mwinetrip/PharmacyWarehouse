using System;

namespace PharmacyWarehouse.Models;

// Класс: Лекарство
public class Medicine
{
    public int Id { get; set; } // Уникальный идентификатор лекарства

    public string Name { get; set; } = string.Empty; // Название лекарства
    public string Category { get; set; } = string.Empty; // Категория лекарства

    public DateTime ManufactureDate { get; set; } // Дата производства
    public DateTime ExpirationDate { get; set; } // Срок годности

    public string RegistrationNumber { get; set; } = string.Empty; // Регистрационный номер Минздрава РФ
    public string Manufacturer { get; set; } = string.Empty; // Производитель
    public string PackageType { get; set; } = string.Empty; // Вид упаковки
    
    // Проверяет, просрочено ли лекарство
    public bool IsExpired() => DateTime.Now.Date > ExpirationDate.Date;
    
    // Проверяет, сколько осталось дней до истечения срока годности
    public int DaysToExpiration => (ExpirationDate.Date - DateTime.Now.Date).Days;

    public override string ToString() =>
        $"{Name} ({Category}) — {Manufacturer}, до {ExpirationDate:dd.MM.yyyy}";
}