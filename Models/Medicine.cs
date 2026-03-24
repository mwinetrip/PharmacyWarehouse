using System;

namespace PharmacyWarehouse.Models;

// Лекарство
public class Medicine
{
    public int Id { get; set; } // Идентификатор
    
    public string Name { get; set; } // Название лекарства
    public string Category { get; set; } // Категория лекарства
    
    public DateTime ManufactureDate { get; set; } // Дата производства
    public DateTime ExpirationDate { get; set; } // Срок годности
    
    public string RegistrationNumber { get; set; } // Регистрационный номер Минздрава
    public string Manufacturer { get; set; } // Производитель
    public string PackageType { get; set; } // Вид упаковки
    
    //??
    public bool IsExpired()
    {
        return DateTime.Now.Date > ExpirationDate.Date;
    }
    
    //??
    public override string ToString()
    {
        return $"{Name} ({Category})";
    }
}