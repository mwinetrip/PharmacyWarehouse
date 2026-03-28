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

    private async void AddItem_Click(object? sender, RoutedEventArgs e)
    {
        if (MedicineComboBox.SelectedItem is not Medicine selectedMedicine)
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Выберите лекарство!");
            return;
        }

        if (selectedMedicine.IsExpired)
        {
            await MessageBoxService.ShowErrorAsync(this, "Запрещено", 
                $"Лекарство '{selectedMedicine.Name}' ПРОСРОЧЕНО!\n\nПродажа запрещена.");
            return;
        }

        if (selectedMedicine.DaysToExpiration <= 0)
        {
            await MessageBoxService.ShowErrorAsync(this, "Запрещено", 
                $"Лекарство '{selectedMedicine.Name}' уже просрочено!");
            return;
        }

        // Предупреждение при небольшом остатке срока годности
        if (selectedMedicine.DaysToExpiration <= 30)
        {
            bool confirmed = await MessageBoxService.ShowWarningAsync(this, "Предупреждение", 
                $"Лекарство '{selectedMedicine.Name}' истекает через {selectedMedicine.DaysToExpiration} дней.\n\nВы уверены, что хотите добавить его в продажу?");

            if (!confirmed) return;
        }

        if (!int.TryParse(QuantityBox.Text, out int quantity) || quantity <= 0)
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Количество должно быть положительным числом!");
            QuantityBox.Focus();
            return;
        }

        if (!decimal.TryParse(PriceBox.Text, out decimal price) || price <= 0)
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Цена должна быть положительной!");
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
}