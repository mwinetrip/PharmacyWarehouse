namespace PharmacyWarehouse.Models;

// Позиция лекарства в накладной
public class InvoiceItem
{
    public Medicine Medicine { get; set; } // Лекарство
    public int Quantity { get; set; } // Количество одного вида лекарства
    public decimal Price { get; set; } // Цена за единицу
    
    // Общая стоимость
    public decimal GetTotalPrice()
    {
        return Price * Quantity;
    }
}