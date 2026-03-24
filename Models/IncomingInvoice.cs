using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyWarehouse.Models;

// Приходная накладная (поступление от поставщика)
public class IncomingInvoice
{
    public int Id { get; set; } // Уникальный идентификатор накладной
    public string InvoiceNumber { get; set; } = string.Empty; // Номер приходной накладной

    public DateTime ReceiptDate { get; set; } = DateTime.Now; // Дата поступления на склад

    public int SupplierId { get; set; } // ID поставщика
    public Supplier? Supplier { get; set; } // Ссылка на поставщика

    public List<InvoiceItem> Items { get; set; } = new(); // Список позиций в накладной

    public decimal TotalAmount => Items.Sum(i => i.Total); // Общая сумма по накладной

    public string SellerName { get; set; } = string.Empty; // ФИО сотрудника, принявшего товар
}