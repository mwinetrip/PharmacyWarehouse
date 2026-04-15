using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;

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
            await ShowErrorAsync("Выберите лекарство!");
            return;
        }

        if (!int.TryParse(QuantityBox.Text, out int quantity) || quantity <= 0)
        {
            await ShowErrorAsync("Количество должно быть положительным числом!");
            QuantityBox.Focus();
            return;
        }

        if (!decimal.TryParse(PriceBox.Text, out decimal price) || price <= 0)
        {
            await ShowErrorAsync("Цена должна быть положительной!");
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
        stack.Children.Add(new TextBlock 
        { 
            Text = message, 
            TextWrapping = TextWrapping.Wrap 
        });

        var btn = new Button 
        { 
            Content = "OK", 
            Width = 100, 
            Height = 35 
        };
        btn.Click += (_, _) => dialog.Close();

        stack.Children.Add(btn);
        dialog.Content = stack;

        await dialog.ShowDialog(this);
    }
}