using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;

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

        LoadMedicines();
    }

    private void LoadMedicines()
    {
        MedicineComboBox.ItemsSource = _dataManager.Medicines;
        if (_dataManager.Medicines.Count > 0)
            MedicineComboBox.SelectedIndex = 0;
    }

    private void AddItem_Click(object? sender, RoutedEventArgs e)
    {
        if (MedicineComboBox.SelectedItem is not Medicine selectedMedicine)
        {
            ShowError("Выберите лекарство!");
            return;
        }

        if (!int.TryParse(QuantityBox.Text, out int quantity) || quantity <= 0)
        {
            ShowError("Количество должно быть положительным числом!");
            QuantityBox.Focus();
            return;
        }

        if (!decimal.TryParse(PriceBox.Text, out decimal price) || price <= 0)
        {
            ShowError("Цена должна быть положительной!");
            PriceBox.Focus();
            return;
        }

        var item = new InvoiceItem
        {
            MedicineId = selectedMedicine.Id,
            Medicine = selectedMedicine,
            Quantity = quantity,
            Price = price
        };

        _onItemAdded(item);
        Close();
    }

    private void ShowError(string message)
    {
        var msgBox = new Window
        {
            Title = "Ошибка",
            Width = 400,
            Height = 150,
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