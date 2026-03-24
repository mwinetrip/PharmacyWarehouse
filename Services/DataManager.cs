using System.Collections.Generic;
using PharmacyWarehouse.Models;

namespace PharmacyWarehouse.Services;

// Управление всеми данными системы
public class DataManager
{
    public List<Medicine> Medicines { get; set; }
    public List<Supplier> Suppliers { get; set; }
    public List<Customer> Customers { get; set; }

    public List<IncomingInvoice> IncomingInvoices { get; set; }
    public List<SalesInvoice> SalesInvoices { get; set; }

    private FileStorageService<Medicine> medicineStorage;
    private FileStorageService<Supplier> supplierStorage;
    private FileStorageService<Customer> customerStorage;
    private FileStorageService<IncomingInvoice> incomingInvoiceStorage;
    private FileStorageService<SalesInvoice> salesInvoiceStorage;

    
    // Менеджер данных
    public DataManager()
    {
        medicineStorage = new FileStorageService<Medicine>("medicines.json");
        supplierStorage = new FileStorageService<Supplier>("suppliers.json");
        customerStorage = new FileStorageService<Customer>("customers.json");
        incomingInvoiceStorage = new FileStorageService<IncomingInvoice>("incomingInvoice.json");
        salesInvoiceStorage = new FileStorageService<SalesInvoice>("salesInvoice.json");

        Medicines = medicineStorage.Load();
        Suppliers = supplierStorage.Load();
        Customers = customerStorage.Load();
        IncomingInvoices = incomingInvoiceStorage.Load();
        SalesInvoices = salesInvoiceStorage.Load();
    }
    
    // Сохранение всех данных
    public void SaveAll()
    {
        medicineStorage.Save(Medicines);
        supplierStorage.Save(Suppliers);
        customerStorage.Save(Customers);
        incomingInvoiceStorage.Save(IncomingInvoices);
        salesInvoiceStorage.Save(SalesInvoices);
    }
}