using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.Views;

public partial class AddSalesInvoiceWindow : Window
{
    private readonly DataManager _dataManager;
    private readonly SalesInvoice? _invoiceToEdit;
    private readonly bool _isEditMode;

    public AddSalesInvoiceWindow(SalesInvoice? invoiceToEdit = null)
    {
        InitializeComponent();
        _dataManager = DataManager.Instance;
        _invoiceToEdit = invoiceToEdit;
        _isEditMode = invoiceToEdit != null;

        Title = _isEditMode ? "Редактирование счёта-фактуры" : "Выписка счёта-фактуры";
        DataContext = this;

        IssueDatePicker.SelectedDate = DateTime.Now;
        CustomerComboBox.ItemsSource = _dataManager.Customers;

        if (_isEditMode && _invoiceToEdit != null)
            LoadExistingInvoiceData();
        else if (_dataManager.Customers.Count > 0)
            CustomerComboBox.SelectedIndex = 0;
    }

    public ObservableCollection<InvoiceItem> CurrentItems { get; } = new();

    private void LoadExistingInvoiceData()
    {
        InvoiceNumberBox.Text = _invoiceToEdit!.InvoiceNumber;
        IssueDatePicker.SelectedDate = _invoiceToEdit.IssueDate;
        SellerNameBox.Text = _invoiceToEdit.SellerName;
        CustomerComboBox.SelectedItem = _dataManager.Customers.FirstOrDefault(c => c.Id == _invoiceToEdit.CustomerId);

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
            await DialogHelper.ShowErrorAsync("Укажите номер счёта-фактуры!", this);
            InvoiceNumberBox.Focus();
            return;
        }

        if (CustomerComboBox.SelectedItem is not Customer selectedCustomer)
        {
            await DialogHelper.ShowErrorAsync("Выберите покупателя!", this);
            return;
        }

        if (string.IsNullOrWhiteSpace(SellerNameBox.Text))
        {
            await DialogHelper.ShowErrorAsync("Укажите фамилию продавца!", this);
            SellerNameBox.Focus();
            return;
        }

        if (CurrentItems.Count == 0)
        {
            await DialogHelper.ShowErrorAsync("Добавьте хотя бы одну позицию!", this);
            return;
        }

        var invoice = new SalesInvoice
        {
            InvoiceNumber = InvoiceNumberBox.Text.Trim(),
            IssueDate = IssueDatePicker.SelectedDate?.DateTime ?? DateTime.Now,
            CustomerId = selectedCustomer.Id,
            Customer = selectedCustomer,
            Items = new List<InvoiceItem>(CurrentItems),
            SellerName = SellerNameBox.Text.Trim()
        };

        if (_isEditMode && _invoiceToEdit != null)
        {
            invoice.Id = _invoiceToEdit.Id;
            _dataManager.UpdateSalesInvoice(invoice);
        }
        else
        {
            _dataManager.AddSalesInvoice(invoice);
        }

        Close();
    }
}
