using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;
using System.Threading.Tasks;

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
            ShowError("Выберите лекарство!");
            return;
        }

        // === КРИТИЧЕСКАЯ ПРОВЕРКА НА ПРОСРОЧКУ ===
        if (selectedMedicine.IsExpired)
        {
            ShowError($"Лекарство '{selectedMedicine.Name}' ПРОСРОЧЕНО!\n\nПродажа запрещена.");
            return;
        }

        if (selectedMedicine.DaysToExpiration <= 0)
        {
            ShowError($"Лекарство '{selectedMedicine.Name}' уже просрочено или истекает сегодня!");
            return;
        }

        if (selectedMedicine.DaysToExpiration <= 30)
        {
            var result = await ShowWarning($"Лекарство '{selectedMedicine.Name}' истекает через {selectedMedicine.DaysToExpiration} дней.\n\nВы уверены, что хотите добавить его в продажу?");
            if (!result) return;
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

    private async Task<bool> ShowWarning(string message)
    {
        // Пока используем простое окно. Позже можно улучшить.
        var msgBox = new Window
        {
            Title = "Предупреждение",
            Width = 450,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new TextBlock 
            { 
                Text = message + "\n\nНажмите OK, чтобы продолжить, или Cancel, чтобы отменить.",
                Margin = new Avalonia.Thickness(20),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap 
            }
        };

        // Здесь пока просто возвращаем true. Полноценный диалог с Yes/No сделаем позже.
        await msgBox.ShowDialog(this);
        return true; 
    }

    private void ShowError(string message)
    {
        var msgBox = new Window
        {
            Title = "Ошибка",
            Width = 420,
            Height = 180,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new TextBlock 
            { 
                Text = message, 
                Margin = new Avalonia.Thickness(25),
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                FontSize = 14
            }
        };
        msgBox.ShowDialog(this);
    }
}