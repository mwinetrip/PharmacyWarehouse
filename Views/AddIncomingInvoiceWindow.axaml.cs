using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using PharmacyWarehouse.ViewModels;

namespace PharmacyWarehouse.Views;

public partial class AddIncomingInvoiceWindow : Window
{
    private readonly DataManager _dataManager;

    public ObservableCollection<InvoiceItem> CurrentItems { get; } = new();

    public AddIncomingInvoiceWindow()
    {
        InitializeComponent();
        _dataManager = DataManager.Instance;
        DataContext = this;

        ReceiptDatePicker.SelectedDate = DateTime.Now;
        LoadSuppliers();
    }

    private void LoadSuppliers()
    {
        SupplierComboBox.ItemsSource = _dataManager.Suppliers;
        if (_dataManager.Suppliers.Count > 0)
            SupplierComboBox.SelectedIndex = 0;
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
            await ShowErrorAsync("Укажите номер приходной накладной!");
            InvoiceNumberBox.Focus();
            return;
        }

        if (SupplierComboBox.SelectedItem is not Supplier selectedSupplier)
        {
            await ShowErrorAsync("Выберите поставщика!");
            return;
        }

        if (CurrentItems.Count == 0)
        {
            await ShowErrorAsync("Добавьте хотя бы одну позицию в накладную!");
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

        if (VisualRoot is MainWindow main && main.DataContext is MainWindowViewModel vm)
            vm.RefreshAll();

        Close();
    }

    private async Task ShowErrorAsync(string message)
    {
        var dialog = new Window
        {
            Title = "Ошибка",
            Width = 420,
            Height = 170,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var stack = new StackPanel { Margin = new Thickness(25), Spacing = 20 };
        stack.Children.Add(new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap });

        var btn = new Button { Content = "OK", Width = 100, Height = 35 };
        btn.Click += (_, _) => dialog.Close();

        stack.Children.Add(btn);
        dialog.Content = stack;

        await dialog.ShowDialog(this);
    }
}