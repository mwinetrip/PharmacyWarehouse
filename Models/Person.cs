namespace PharmacyWarehouse.Models;

// Базовый класс: Человек
public abstract class Person
{
    public int Id { get; set; } // Идентификатор
    public string Name { get; set; } // Имя
    public string Address { get; set; } // Адрес
    public string Phone { get; set; } // Номер телефона
    public string Inn { get; set; } // ИНН
    
    // Информация о человеке
    public virtual string GetInfo()
    {
        return $"Id: {Id}, Name: {Name}, Address: {Address}, Phone: {Phone}";
    }
}