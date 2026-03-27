using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;

namespace PharmacyWarehouse.Views;

public partial class AddIncomingInvoiceWindow : Window
{
    private readonly DataManager _dataManager;

    public AddIncomingInvoiceWindow()
    {
        InitializeComponent();
        _dataManager = new DataManager();

        ReceiptDatePicker.SelectedDate = DateTime.Now;
        LoadSuppliers();
    }

    private void LoadSuppliers()
    {
        SupplierComboBox.ItemsSource = _dataManager.Suppliers;
        SupplierComboBox.SelectedIndex = 0; // если есть поставщики
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(InvoiceNumberBox.Text))
        {
            ShowError("Укажите номер накладной!");
            return;
        }

        if (SupplierComboBox.SelectedItem is not Supplier selectedSupplier)
        {
            ShowError("Выберите поставщика!");
            return;
        }

        // Пока создаём пустую накладную (позиции добавим позже)
        var invoice = new IncomingInvoice
        {
            InvoiceNumber = InvoiceNumberBox.Text.Trim(),
            ReceiptDate = ReceiptDatePicker.SelectedDate?.DateTime ?? DateTime.Now,
            SupplierId = selectedSupplier.Id,
            Supplier = selectedSupplier,
            SellerName = "Администратор" // пока жёстко
        };

        // _dataManager.AddIncomingInvoice(invoice);  // добавим метод позже в DataManager

        Close();
    }

    private void ShowError(string message)
    {
        var msgBox = new Window
        {
            Title = "Ошибка",
            Width = 420,
            Height = 160,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new TextBlock 
            { 
                Text = message, 
                Margin = new Avalonia.Thickness(20),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap 
            }
        };
        msgBox.ShowDialog(this);
    }
}