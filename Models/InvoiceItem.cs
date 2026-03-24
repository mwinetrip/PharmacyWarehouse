namespace PharmacyWarehouse.Models;

// Позиция в накладной (приход или расход)
public class InvoiceItem
{
    public int Id { get; set; } // Уникальный идентификатор позиции

    public int MedicineId { get; set; } // Уникальный идентификатор лекарства
    public Medicine? Medicine { get; set; } // Навигационное свойство (ссылка на лекарство)

    public decimal Price { get; set; } // Цена за единицу
    public int Quantity { get; set; } // Количество

    public decimal Total => Price * Quantity; // Общая стоимость

    public override string ToString() =>
        Medicine != null 
            ? $"{Medicine.Name} — {Quantity} шт. по {Price} ₽" 
            : $"Позиция {MedicineId} — {Quantity} шт.";
}