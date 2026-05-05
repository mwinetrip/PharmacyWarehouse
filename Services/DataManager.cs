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
    private static DataManager? _instance;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private DataManager()
    {
        Directory.CreateDirectory(DataFolder);
        LoadAll();
    }

    public static DataManager Instance => _instance ??= new DataManager();

    public ObservableCollection<Medicine> Medicines { get; private set; } = new();
    public ObservableCollection<Supplier> Suppliers { get; private set; } = new();
    public ObservableCollection<Customer> Customers { get; private set; } = new();
    public ObservableCollection<IncomingInvoice> IncomingInvoices { get; private set; } = new();
    public ObservableCollection<SalesInvoice> SalesInvoices { get; private set; } = new();

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

        foreach (var invoice in IncomingInvoices)
        {
            invoice.Supplier = supplierDict.TryGetValue(invoice.SupplierId, out var s) ? s : null;
            foreach (var item in invoice.Items)
                item.Medicine = medicineDict.TryGetValue(item.MedicineId, out var m) ? m : null;
        }

        foreach (var invoice in SalesInvoices)
        {
            invoice.Customer = customerDict.TryGetValue(invoice.CustomerId, out var c) ? c : null;
            foreach (var item in invoice.Items)
                item.Medicine = medicineDict.TryGetValue(item.MedicineId, out var m) ? m : null;
        }
    }

    // ==================== CRUD для справочников ====================

    public void AddMedicine(Medicine medicine)
    {
        medicine.Id = Medicines.Count > 0 ? Medicines.Max(m => m.Id) + 1 : 1;
        Medicines.Add(medicine);
        SaveAll();
    }

    public void UpdateMedicine(Medicine updated)
    {
        var item = Medicines.FirstOrDefault(x => x.Id == updated.Id);
        if (item == null) return;

        item.Name = updated.Name;
        item.Category = updated.Category;
        item.Manufacturer = updated.Manufacturer;
        item.PackageType = updated.PackageType;
        item.RegistrationNumber = updated.RegistrationNumber;
        item.ManufactureDate = updated.ManufactureDate;
        item.ExpirationDate = updated.ExpirationDate;

        SaveAll();
    }

    public void AddSupplier(Supplier supplier)
    {
        supplier.Id = Suppliers.Count > 0 ? Suppliers.Max(s => s.Id) + 1 : 1;
        Suppliers.Add(supplier);
        SaveAll();
    }

    public void UpdateSupplier(Supplier updated)
    {
        var item = Suppliers.FirstOrDefault(x => x.Id == updated.Id);
        if (item == null) return;

        item.Name = updated.Name;
        item.Inn = updated.Inn;
        item.Address = updated.Address;
        item.Phone = updated.Phone;
        item.Bank = updated.Bank;
        item.AccountNumber = updated.AccountNumber;

        SaveAll();
    }

    public void AddCustomer(Customer customer)
    {
        customer.Id = Customers.Count > 0 ? Customers.Max(c => c.Id) + 1 : 1;
        Customers.Add(customer);
        SaveAll();
    }

    public void UpdateCustomer(Customer updated)
    {
        var item = Customers.FirstOrDefault(x => x.Id == updated.Id);
        if (item == null) return;

        item.Name = updated.Name;
        item.Inn = updated.Inn;
        item.Address = updated.Address;
        item.Phone = updated.Phone;

        SaveAll();
    }

    // ==================== CRUD для документов ====================

    public void AddIncomingInvoice(IncomingInvoice invoice)
    {
        invoice.Id = IncomingInvoices.Count > 0 ? IncomingInvoices.Max(i => i.Id) + 1 : 1;
        IncomingInvoices.Add(invoice);
        SaveAll();
        LinkAllEntities();
    }

    public void UpdateIncomingInvoice(IncomingInvoice updated)
    {
        var invoice = IncomingInvoices.FirstOrDefault(x => x.Id == updated.Id);
        if (invoice == null) return;

        invoice.InvoiceNumber = updated.InvoiceNumber;
        invoice.ReceiptDate = updated.ReceiptDate;
        invoice.SupplierId = updated.SupplierId;
        invoice.Supplier = updated.Supplier;
        invoice.SellerName = updated.SellerName;
        invoice.Items = new List<InvoiceItem>(updated.Items);

        SaveAll();
        LinkAllEntities();
    }

    public void AddSalesInvoice(SalesInvoice invoice)
    {
        invoice.Id = SalesInvoices.Count > 0 ? SalesInvoices.Max(i => i.Id) + 1 : 1;
        SalesInvoices.Add(invoice);
        SaveAll();
        LinkAllEntities();
    }

    public void UpdateSalesInvoice(SalesInvoice updated)
    {
        var invoice = SalesInvoices.FirstOrDefault(x => x.Id == updated.Id);
        if (invoice == null) return;

        invoice.InvoiceNumber = updated.InvoiceNumber;
        invoice.IssueDate = updated.IssueDate;
        invoice.CustomerId = updated.CustomerId;
        invoice.Customer = updated.Customer;
        invoice.SellerName = updated.SellerName;
        invoice.Items = new List<InvoiceItem>(updated.Items);

        SaveAll();
        LinkAllEntities();
    }
}