using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PharmacyWarehouse.ViewModels;

namespace PharmacyWarehouse.Views;

public partial class AddIncomingInvoiceWindow : Window, INotifyPropertyChanged
{
    private readonly DataManager _dataManager;

    private ObservableCollection<InvoiceItem> _currentItems = new();
    public ObservableCollection<InvoiceItem> CurrentItems
    {
        get => _currentItems;
        set
        {
            if (_currentItems != value)
            {
                _currentItems = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public AddIncomingInvoiceWindow()
    {
        InitializeComponent();
        
        _dataManager = new DataManager();
        DataContext = this;                    // Обязательно для Binding CurrentItems

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
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Укажите номер приходной накладной!");
            InvoiceNumberBox.Focus();
            return;
        }

        if (SupplierComboBox.SelectedItem is not Supplier selectedSupplier)
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Выберите поставщика!");
            return;
        }

        if (CurrentItems.Count == 0)
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Добавьте хотя бы одну позицию в накладную!");
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
        
        if (VisualRoot is MainWindow mainWindow && mainWindow.DataContext is MainWindowViewModel vm)
        {
            vm.RefreshAll();
        }
        
        Close();
    }
}