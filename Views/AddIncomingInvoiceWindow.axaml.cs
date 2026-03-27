using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace PharmacyWarehouse.Views;

public partial class AddIncomingInvoiceWindow : Window
{
    private readonly DataManager _dataManager;
    public ObservableCollection<InvoiceItem> CurrentItems { get; } = new();
    private void AddItem_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new AddInvoiceItemDialog(_dataManager, item => CurrentItems.Add(item));
        dialog.ShowDialog(this);
    }
    
    public AddIncomingInvoiceWindow()
    {
        InitializeComponent();
        _dataManager = new DataManager();

        ReceiptDatePicker.SelectedDate = DateTime.Now;
        LoadSuppliers();
        // LoadMedicines(); // можно добавить позже для выбора лекарства
    }

    private void LoadSuppliers()
    {
        SupplierComboBox.ItemsSource = _dataManager.Suppliers;
        if (_dataManager.Suppliers.Count > 0)
            SupplierComboBox.SelectedIndex = 0;
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(InvoiceNumberBox.Text))
        {
            ShowError("Укажите номер приходной накладной!");
            InvoiceNumberBox.Focus();
            return;
        }

        if (SupplierComboBox.SelectedItem is not Supplier selectedSupplier)
        {
            ShowError("Выберите поставщика!");
            return;
        }

        if (CurrentItems.Count == 0)
        {
            ShowError("Добавьте хотя бы одну позицию в накладную!");
            return;
        }

        var invoice = new IncomingInvoice
        {
            InvoiceNumber = InvoiceNumberBox.Text.Trim(),
            ReceiptDate = ReceiptDatePicker.SelectedDate?.DateTime ?? DateTime.Now,
            SupplierId = selectedSupplier.Id,
            Supplier = selectedSupplier,
            Items = new List<InvoiceItem>(CurrentItems),
            SellerName = "Администратор"
        };

        _dataManager.AddIncomingInvoice(invoice);
        Close();
    }

    private void ShowError(string message)
    {
        var msgBox = new Window
        {
            Title = "Ошибка ввода",
            Width = 420,
            Height = 170,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new TextBlock 
            { 
                Text = message, 
                Margin = new Avalonia.Thickness(25),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                FontSize = 14
            }
        };
        msgBox.ShowDialog(this);
    }
}