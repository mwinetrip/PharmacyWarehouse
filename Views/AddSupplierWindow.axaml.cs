using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;
using PharmacyWarehouse.ViewModels;

namespace PharmacyWarehouse.Views;

public partial class AddSupplierWindow : Window
{
    private readonly DataManager _dataManager;

    public AddSupplierWindow()
    {
        InitializeComponent();
        _dataManager = DataManager.Instance;
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Название поставщика обязательно!");
            NameBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(InnBox.Text) || !IsValidInn(InnBox.Text))
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "ИНН должен содержать 10 или 12 цифр и быть корректным!");
            InnBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(PhoneBox.Text))
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Укажите телефон!");
            PhoneBox.Focus();
            return;
        }

        var supplier = new Supplier
        {
            Name = NameBox.Text.Trim(),
            Inn = InnBox.Text.Trim(),
            Address = AddressBox.Text?.Trim() ?? "",
            Phone = PhoneBox.Text.Trim(),
            Bank = BankBox.Text?.Trim() ?? "",
            AccountNumber = AccountBox.Text?.Trim() ?? ""
        };

        _dataManager.AddSupplier(supplier);
        
        if (VisualRoot is MainWindow mainWindow && mainWindow.DataContext is MainWindowViewModel vm)
        {
            vm.RefreshAll();
        }
        
        Close();
    }

    private bool IsValidInn(string inn)
    {
        if (string.IsNullOrWhiteSpace(inn)) return false;
        inn = inn.Trim();
        if (inn.Length != 10 && inn.Length != 12) return false;
        return long.TryParse(inn, out _);
    }
}