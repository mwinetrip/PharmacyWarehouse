using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace PharmacyWarehouse.Views;

public partial class AddSalesInvoiceWindow : Window
{
    private readonly DataManager _dataManager;
    private readonly ObservableCollection<InvoiceItem> _currentItems = new();

    public AddSalesInvoiceWindow()
    {
        InitializeComponent();
        _dataManager = new DataManager();

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
        var dialog = new AddInvoiceItemDialog(_dataManager, item => _currentItems.Add(item));
        dialog.ShowDialog(this);
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(InvoiceNumberBox.Text))
        {
            ShowError("Укажите номер счёта!");
            return;
        }

        if (CustomerComboBox.SelectedItem is not Customer selectedCustomer)
        {
            ShowError("Выберите покупателя!");
            return;
        }

        if (string.IsNullOrWhiteSpace(SellerNameBox.Text))
        {
            ShowError("Укажите фамилию продавца!");
            SellerNameBox.Focus();
            return;
        }

        if (_currentItems.Count == 0)
        {
            ShowError("Добавьте хотя бы одну позицию!");
            return;
        }

        var invoice = new SalesInvoice
        {
            InvoiceNumber = InvoiceNumberBox.Text.Trim(),
            IssueDate = IssueDatePicker.SelectedDate?.DateTime ?? DateTime.Now,
            CustomerId = selectedCustomer.Id,
            Customer = selectedCustomer,
            Items = new List<InvoiceItem>(_currentItems),
            SellerName = SellerNameBox.Text.Trim()
        };

        _dataManager.AddSalesInvoice(invoice);

        Close();
    }

    private void ShowError(string message)
    {
        var msgBox = new Window
        {
            Title = "Ошибка",
            Width = 420,
            Height = 170,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new TextBlock 
            { 
                Text = message, 
                Margin = new Avalonia.Thickness(25),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap 
            }
        };
        msgBox.ShowDialog(this);
    }
}