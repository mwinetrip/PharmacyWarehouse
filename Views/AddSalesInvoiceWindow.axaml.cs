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

public partial class AddSalesInvoiceWindow : Window, INotifyPropertyChanged
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

    public AddSalesInvoiceWindow()
    {
        InitializeComponent();
        
        _dataManager = new DataManager();
        DataContext = this;                    // Важно для {Binding CurrentItems}

        IssueDatePicker.SelectedDate = DateTime.Now;
        LoadCustomers();
    }

    private void LoadCustomers()
    {
        CustomerComboBox.ItemsSource = _dataManager.Customers;
        if (_dataManager.Customers.Count > 0)
            CustomerComboBox.SelectedIndex = 0;
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
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Укажите номер счёта-фактуры!");
            InvoiceNumberBox.Focus();
            return;
        }

        if (CustomerComboBox.SelectedItem is not Customer selectedCustomer)
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Выберите покупателя!");
            return;
        }

        if (string.IsNullOrWhiteSpace(SellerNameBox.Text))
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Укажите фамилию продавца!");
            SellerNameBox.Focus();
            return;
        }

        if (CurrentItems.Count == 0)
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Добавьте хотя бы одну позицию в счёт!");
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

        _dataManager.AddSalesInvoice(invoice);
        
        if (VisualRoot is MainWindow mainWindow && mainWindow.DataContext is MainWindowViewModel vm)
        {
            vm.RefreshAll();
        }
        
        Close();
    }
}