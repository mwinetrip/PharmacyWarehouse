using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyWarehouse.Models;

// Счёт-фактура (отпуск покупателю)
public class SalesInvoice
{
    public int Id { get; set; } // Уникальный идентификатор счёта
    public string InvoiceNumber { get; set; } = string.Empty; // Номер счёта-фактуры

    public DateTime IssueDate { get; set; } = DateTime.Now; // Дата выписки счёта

    public int CustomerId { get; set; } // ID покупателя
    public Customer? Customer { get; set; } // Ссылка на покупателя

    public List<InvoiceItem> Items { get; set; } = new(); // Список позиций в счёте

    public decimal TotalAmount => Items.Sum(i => i.Total); // Общая сумма к оплате

    public string SellerName { get; set; } = string.Empty;   // Фамилия продавца, выписавшего счёт
}