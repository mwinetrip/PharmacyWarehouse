using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using PharmacyWarehouse.Models;

namespace PharmacyWarehouse.Services;

public class DataManager
{
    private const string DataFolder = "Data";
    
    public ObservableCollection<Medicine> Medicines { get; private set; } = new();
    public ObservableCollection<Supplier> Suppliers { get; private set; } = new();
    public ObservableCollection<Customer> Customers { get; private set; } = new();
    public ObservableCollection<IncomingInvoice> IncomingInvoices { get; private set; } = new();
    public ObservableCollection<SalesInvoice> SalesInvoices { get; private set; } = new();

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles // на всякий случай
    };

    public DataManager()
    {
        Directory.CreateDirectory(DataFolder);
        LoadAll();
    }

    public void SaveAll()
    {
        SaveCollection(Medicines, "medicines.json");
        SaveCollection(Suppliers, "suppliers.json");
        SaveCollection(Customers, "customers.json");
        SaveCollection(IncomingInvoices, "incoming_invoices.json");
        SaveCollection(SalesInvoices, "sales_invoices.json");
    }

    private void SaveCollection<T>(IEnumerable<T> collection, string fileName)
    {
        var path = Path.Combine(DataFolder, fileName);
        var json = JsonSerializer.Serialize(collection, _jsonOptions);
        File.WriteAllText(path, json);
    }

    private void LoadAll()
    {
        Medicines = LoadCollection<Medicine>("medicines.json");
        Suppliers = LoadCollection<Supplier>("suppliers.json");
        Customers = LoadCollection<Customer>("customers.json");
        IncomingInvoices = LoadCollection<IncomingInvoice>("incoming_invoices.json");
        SalesInvoices = LoadCollection<SalesInvoice>("sales_invoices.json");

        // === КРИТИЧНОЕ ИСПРАВЛЕНИЕ: восстановление навигационных свойств ===
        LinkAllEntities();
    }

    private ObservableCollection<T> LoadCollection<T>(string fileName)
    {
        var path = Path.Combine(DataFolder, fileName);
        if (!File.Exists(path)) return new ObservableCollection<T>();

        try
        {
            var json = File.ReadAllText(path);
            var list = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
            return new ObservableCollection<T>(list);
        }
        catch
        {
            return new ObservableCollection<T>();
        }
    }

    private void LinkAllEntities()
    {
        var medicineDict = Medicines.ToDictionary(m => m.Id);
        var supplierDict = Suppliers.ToDictionary(s => s.Id);
        var customerDict = Customers.ToDictionary(c => c.Id);

        // Связываем приходные накладные
        foreach (var invoice in IncomingInvoices)
        {
            invoice.Supplier = supplierDict.TryGetValue(invoice.SupplierId, out var s) ? s : null;
            foreach (var item in invoice.Items)
            {
                item.Medicine = medicineDict.TryGetValue(item.MedicineId, out var m) ? m : null;
            }
        }

        // Связываем счета-фактуры
        foreach (var invoice in SalesInvoices)
        {
            invoice.Customer = customerDict.TryGetValue(invoice.CustomerId, out var c) ? c : null;
            foreach (var item in invoice.Items)
            {
                item.Medicine = medicineDict.TryGetValue(item.MedicineId, out var m) ? m : null;
            }
        }
    }

    // ====================== Добавление ======================
    public void AddMedicine(Medicine medicine)
    {
        medicine.Id = Medicines.Count > 0 ? Medicines.Max(m => m.Id) + 1 : 1;
        Medicines.Add(medicine);
        SaveAll();
    }

    public void AddSupplier(Supplier supplier)
    {
        supplier.Id = Suppliers.Count > 0 ? Suppliers.Max(s => s.Id) + 1 : 1;
        Suppliers.Add(supplier);
        SaveAll();
    }

    public void AddCustomer(Customer customer)
    {
        customer.Id = Customers.Count > 0 ? Customers.Max(c => c.Id) + 1 : 1;
        Customers.Add(customer);
        SaveAll();
    }

    public void AddIncomingInvoice(IncomingInvoice invoice)
    {
        invoice.Id = IncomingInvoices.Count > 0 ? IncomingInvoices.Max(i => i.Id) + 1 : 1;
        IncomingInvoices.Add(invoice);
        SaveAll();
        LinkAllEntities(); // обновляем связи
    }

    public void AddSalesInvoice(SalesInvoice invoice)
    {
        invoice.Id = SalesInvoices.Count > 0 ? SalesInvoices.Max(i => i.Id) + 1 : 1;
        SalesInvoices.Add(invoice);
        SaveAll();
        LinkAllEntities();
    }
}