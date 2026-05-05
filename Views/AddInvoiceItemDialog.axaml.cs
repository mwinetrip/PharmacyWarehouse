using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.Views;

public partial class AddInvoiceItemDialog : Window
{
    private readonly DataManager _dataManager;
    private readonly Action<InvoiceItem> _onItemAdded;

    public AddInvoiceItemDialog(DataManager dataManager, Action<InvoiceItem> onItemAdded)
    {
        InitializeComponent();
        _dataManager = dataManager;
        _onItemAdded = onItemAdded;

        MedicineComboBox.ItemsSource = _dataManager.Medicines;
        if (_dataManager.Medicines.Count > 0)
            MedicineComboBox.SelectedIndex = 0;
    }

    private async void AddItem_Click(object? sender, RoutedEventArgs e)
    {
        if (MedicineComboBox.SelectedItem is not Medicine selectedMedicine)
        {
            await DialogHelper.ShowErrorAsync("Выберите лекарство!", this);
            return;
        }

        if (!int.TryParse(QuantityBox.Text, out var quantity) || quantity <= 0)
        {
            await DialogHelper.ShowErrorAsync("Количество должно быть положительным числом!", this);
            QuantityBox.Focus();
            return;
        }

        if (!decimal.TryParse(PriceBox.Text?.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var price) || price <= 0)
        {
            await DialogHelper.ShowErrorAsync("Цена должна быть положительной!", this);
            PriceBox.Focus();
            return;
        }

        _onItemAdded(new InvoiceItem
        {
            MedicineId = selectedMedicine.Id,
            Medicine = selectedMedicine,
            Quantity = quantity,
            Price = price
        });

        Close();
    }
}
