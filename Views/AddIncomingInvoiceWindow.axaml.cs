using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.Views;

public partial class AddIncomingInvoiceWindow : Window
{
    private readonly DataManager _dataManager;
    private readonly IncomingInvoice? _invoiceToEdit;
    private readonly bool _isEditMode;

    public AddIncomingInvoiceWindow(IncomingInvoice? invoiceToEdit = null)
    {
        InitializeComponent();
        _dataManager = DataManager.Instance;
        _invoiceToEdit = invoiceToEdit;
        _isEditMode = invoiceToEdit != null;

        Title = _isEditMode ? "Редактирование приходной накладной" : "Регистрация приходной накладной";
        DataContext = this;

        ReceiptDatePicker.SelectedDate = DateTime.Now;
        SupplierComboBox.ItemsSource = _dataManager.Suppliers;

        if (_isEditMode && _invoiceToEdit != null)
            LoadExistingInvoiceData();
        else if (_dataManager.Suppliers.Count > 0)
            SupplierComboBox.SelectedIndex = 0;
    }

    public ObservableCollection<InvoiceItem> CurrentItems { get; } = new();

    private void LoadExistingInvoiceData()
    {
        InvoiceNumberBox.Text = _invoiceToEdit!.InvoiceNumber;
        ReceiptDatePicker.SelectedDate = _invoiceToEdit.ReceiptDate;
        SellerNameBox.Text = _invoiceToEdit.SellerName;
        SupplierComboBox.SelectedItem = _dataManager.Suppliers.FirstOrDefault(s => s.Id == _invoiceToEdit.SupplierId);

        foreach (var item in _invoiceToEdit.Items)
            CurrentItems.Add(new InvoiceItem
            {
                MedicineId = item.MedicineId,
                Medicine = item.Medicine,
                Price = item.Price,
                Quantity = item.Quantity
            });
    }

    private void AddItem_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new AddInvoiceItemDialog(_dataManager, item => CurrentItems.Add(item));
        dialog.ShowDialog(this);
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(InvoiceNumberBox.Text))
        {
            await DialogHelper.ShowErrorAsync("Укажите номер приходной накладной!", this);
            InvoiceNumberBox.Focus();
            return;
        }

        if (SupplierComboBox.SelectedItem is not Supplier selectedSupplier)
        {
            await DialogHelper.ShowErrorAsync("Выберите поставщика!", this);
            return;
        }

        if (string.IsNullOrWhiteSpace(SellerNameBox.Text))
        {
            await DialogHelper.ShowErrorAsync("Укажите ФИО сотрудника, принявшего товар!", this);
            SellerNameBox.Focus();
            return;
        }

        if (CurrentItems.Count == 0)
        {
            await DialogHelper.ShowErrorAsync("Добавьте хотя бы одну позицию в накладную!", this);
            return;
        }

        var invoice = new IncomingInvoice
        {
            InvoiceNumber = InvoiceNumberBox.Text.Trim(),
            ReceiptDate = ReceiptDatePicker.SelectedDate?.DateTime ?? DateTime.Now,
            SupplierId = selectedSupplier.Id,
            Supplier = selectedSupplier,
            Items = new List<InvoiceItem>(CurrentItems),
            SellerName = SellerNameBox.Text.Trim()
        };

        if (_isEditMode && _invoiceToEdit != null)
        {
            invoice.Id = _invoiceToEdit.Id;
            _dataManager.UpdateIncomingInvoice(invoice);
        }
        else
        {
            _dataManager.AddIncomingInvoice(invoice);
        }

        Close();
    }
}
