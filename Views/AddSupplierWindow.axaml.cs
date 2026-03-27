using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;

namespace PharmacyWarehouse.Views;

public partial class AddSupplierWindow : Window
{
    private readonly DataManager _dataManager;

    public AddSupplierWindow()
    {
        InitializeComponent();
        _dataManager = new DataManager();
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            ShowError("Название поставщика обязательно!");
            NameBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(InnBox.Text) || !IsValidInn(InnBox.Text))
        {
            ShowError("ИНН должен содержать 10 или 12 цифр и быть корректным!");
            InnBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(PhoneBox.Text))
        {
            ShowError("Укажите телефон!");
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
        Close();
    }

    private bool IsValidInn(string inn)
    {
        if (string.IsNullOrWhiteSpace(inn)) return false;
        inn = inn.Trim();
        if (inn.Length != 10 && inn.Length != 12) return false;

        return long.TryParse(inn, out _); // простая проверка на цифры
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