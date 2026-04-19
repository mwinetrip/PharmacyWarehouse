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

namespace PharmacyWarehouse.Views.AddWindow;

public partial class AddSalesInvoiceWindow : Window
{
    private readonly DataManager _dataManager;

    public ObservableCollection<InvoiceItem> CurrentItems { get; } = new();

    public AddSalesInvoiceWindow()
    {
        InitializeComponent();
        _dataManager = DataManager.Instance;
        DataContext = this;

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
            await ShowErrorAsync("Укажите номер счёта-фактуры!");
            InvoiceNumberBox.Focus();
            return;
        }

        if (CustomerComboBox.SelectedItem is not Customer selectedCustomer)
        {
            await ShowErrorAsync("Выберите покупателя!");
            return;
        }

        if (string.IsNullOrWhiteSpace(SellerNameBox.Text))
        {
            await ShowErrorAsync("Укажите фамилию продавца!");
            SellerNameBox.Focus();
            return;
        }

        if (CurrentItems.Count == 0)
        {
            await ShowErrorAsync("Добавьте хотя бы одну позицию!");
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