namespace PharmacyWarehouse.Models;

// Базовый класс: Человек
public abstract class Person
{
    public int Id { get; set; } // Уникальный идентификатор
    
    public string Name { get; set; } = string.Empty; // Название организации или ФИО
    public string Address { get; set; } = string.Empty; // Адрес
    public string Phone { get; set; } = string.Empty; // Номер телефона
    public string Inn { get; set; } = string.Empty; // ИНН (10 или 12 цифр)

    public virtual string GetInfo() =>
        $"{Name} (ИНН: {Inn}), тел: {Phone}, адрес: {Address}";
}