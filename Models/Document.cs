using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyWarehouse.Models;

// Базовый класс: Документ
public abstract class Document
{
    public int Id { get; set; } // Идентификатор
    public string Number { get; set; } // Номер документа
    public DateTime Date { get; set; } // Дата документа

    public List<InvoiceItem> Items { get; set; } = new(); // Перечень лекарств
    
    // Общее количество лекарств
    public virtual decimal GetTotalSum()
    {
        return Items.Sum(i => i.GetTotalPrice());
    }
}