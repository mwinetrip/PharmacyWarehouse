using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.Views;

public partial class AddSupplierWindow : Window
{
    private readonly DataManager _dataManager;
    private readonly bool _isEditMode;
    private readonly Supplier? _supplierToEdit;

    public AddSupplierWindow(Supplier? supplierToEdit = null)
    {
        InitializeComponent();
        _dataManager = DataManager.Instance;
        _supplierToEdit = supplierToEdit;
        _isEditMode = supplierToEdit != null;

        Title = _isEditMode ? "Редактирование поставщика" : "Добавление поставщика";

        if (_isEditMode && _supplierToEdit != null)
            LoadSupplierData(_supplierToEdit);
    }

    private void LoadSupplierData(Supplier supplier)
    {
        NameBox.Text = supplier.Name;
        InnBox.Text = supplier.Inn;
        AddressBox.Text = supplier.Address;
        PhoneBox.Text = supplier.Phone;
        BankBox.Text = supplier.Bank;
        AccountBox.Text = supplier.AccountNumber;
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            await DialogHelper.ShowErrorAsync("Название поставщика обязательно!", this);
            NameBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(InnBox.Text) || !IsValidInn(InnBox.Text))
        {
            await DialogHelper.ShowErrorAsync("ИНН должен содержать 10 или 12 цифр!", this);
            InnBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(PhoneBox.Text))
        {
            await DialogHelper.ShowErrorAsync("Укажите телефон!", this);
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

        if (_isEditMode && _supplierToEdit != null)
        {
            supplier.Id = _supplierToEdit.Id;
            _dataManager.UpdateSupplier(supplier);
        }
        else
        {
            _dataManager.AddSupplier(supplier);
        }

        Close();
    }

    private static bool IsValidInn(string inn)
    {
        inn = inn.Trim();
        return (inn.Length == 10 || inn.Length == 12) && long.TryParse(inn, out _);
    }
}
