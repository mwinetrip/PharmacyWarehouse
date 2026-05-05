using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.Views;

public partial class AddCustomerWindow : Window
{
    private readonly Customer? _customerToEdit;
    private readonly DataManager _dataManager;
    private readonly bool _isEditMode;

    public AddCustomerWindow(Customer? customerToEdit = null)
    {
        InitializeComponent();
        _dataManager = DataManager.Instance;
        _customerToEdit = customerToEdit;
        _isEditMode = customerToEdit != null;

        Title = _isEditMode ? "Редактирование покупателя" : "Добавление покупателя";

        if (_isEditMode && _customerToEdit != null)
            LoadCustomerData(_customerToEdit);
    }

    private void LoadCustomerData(Customer customer)
    {
        NameBox.Text = customer.Name;
        InnBox.Text = customer.Inn;
        AddressBox.Text = customer.Address;
        PhoneBox.Text = customer.Phone;
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            await DialogHelper.ShowErrorAsync("Название покупателя обязательно!", this);
            NameBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(InnBox.Text) || !IsValidInn(InnBox.Text))
        {
            await DialogHelper.ShowErrorAsync("ИНН должен содержать 10 или 12 цифр!", this);
            InnBox.Focus();
            return;
        }

        var customer = new Customer
        {
            Name = NameBox.Text.Trim(),
            Inn = InnBox.Text.Trim(),
            Address = AddressBox.Text?.Trim() ?? "",
            Phone = PhoneBox.Text?.Trim() ?? ""
        };

        if (_isEditMode && _customerToEdit != null)
        {
            customer.Id = _customerToEdit.Id;
            _dataManager.UpdateCustomer(customer);
        }
        else
        {
            _dataManager.AddCustomer(customer);
        }

        Close();
    }

    private static bool IsValidInn(string inn)
    {
        inn = inn.Trim();
        return (inn.Length == 10 || inn.Length == 12) && long.TryParse(inn, out _);
    }
}
