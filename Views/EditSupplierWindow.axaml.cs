using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;

namespace PharmacyWarehouse.Views;

public partial class EditSupplierWindow : Window
{
    private readonly DataManager _dataManager;
    private readonly Supplier _supplier;

    public EditSupplierWindow(Supplier supplier)
    {
        InitializeComponent();
        _dataManager = DataManager.Instance;
        _supplier = supplier;

        IdTextBlock.Text = _supplier.Id.ToString();

        NameBox.Text = _supplier.Name;
        InnBox.Text = _supplier.Inn;
        AddressBox.Text = _supplier.Address;
        PhoneBox.Text = _supplier.Phone;
        BankBox.Text = _supplier.Bank;
        AccountBox.Text = _supplier.AccountNumber;
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

        var updated = new Supplier
        {
            Id = _supplier.Id,
            Name = NameBox.Text.Trim(),
            Inn = InnBox.Text.Trim(),
            Address = AddressBox.Text?.Trim() ?? "",
            Phone = PhoneBox.Text.Trim(),
            Bank = BankBox.Text?.Trim() ?? "",
            AccountNumber = AccountBox.Text?.Trim() ?? ""
        };

        if (_supplier.Id == 0)
            _dataManager.AddSupplier(updated);
        else
            _dataManager.UpdateSupplier(updated);
        Close();
    }

    private async void Delete_Click(object? sender, RoutedEventArgs e)
    {
        if (_supplier.Id == 0)
        {
            Close();
            return;
        }

        bool confirmed = await MessageBoxService.ShowWarningAsync(
            this,
            "Удаление",
            $"Удалить поставщика '{_supplier.Name}' (ID: {_supplier.Id})? Восстановление невозможно.");

        if (!confirmed) return;

        _dataManager.DeleteSupplier(_supplier.Id);
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

