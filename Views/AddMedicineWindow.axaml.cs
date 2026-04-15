using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using PharmacyWarehouse.ViewModels;

namespace PharmacyWarehouse.Views;

public partial class AddMedicineWindow : Window
{
    private readonly DataManager _dataManager;

    public AddMedicineWindow()
    {
        InitializeComponent();
        _dataManager = DataManager.Instance;

        ManufactureDatePicker.SelectedDate = DateTime.Now.AddMonths(-3);
        ExpirationDatePicker.SelectedDate = DateTime.Now.AddYears(1);
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            await ShowErrorAsync("Название лекарства обязательно!");
            NameBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(CategoryBox.Text))
        {
            await ShowErrorAsync("Категория лекарства обязательна!");
            CategoryBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(ManufacturerBox.Text))
        {
            await ShowErrorAsync("Укажите производителя!");
            ManufacturerBox.Focus();
            return;
        }

        if (ManufactureDatePicker.SelectedDate == null)
        {
            await ShowErrorAsync("Выберите дату производства!");
            return;
        }

        if (ExpirationDatePicker.SelectedDate == null)
        {
            await ShowErrorAsync("Выберите срок годности!");
            return;
        }

        var manufacture = ManufactureDatePicker.SelectedDate.Value.DateTime.Date;
        var expiration = ExpirationDatePicker.SelectedDate.Value.DateTime.Date;

        if (manufacture > DateTime.Now.Date)
        {
            await ShowErrorAsync("Дата производства не может быть в будущем!");
            return;
        }

        if (expiration <= manufacture)
        {
            await ShowErrorAsync("Срок годности должен быть позже даты производства!");
            return;
        }

        if (string.IsNullOrWhiteSpace(RegNumberBox.Text))
        {
            await ShowErrorAsync("Регистрационный номер Минздрава обязателен!");
            RegNumberBox.Focus();
            return;
        }

        var medicine = new Medicine
        {
            Name = NameBox.Text.Trim(),
            Category = CategoryBox.Text.Trim(),
            Manufacturer = ManufacturerBox.Text.Trim(),
            PackageType = PackageBox.Text?.Trim() ?? "",
            RegistrationNumber = RegNumberBox.Text.Trim(),
            ManufactureDate = manufacture,
            ExpirationDate = expiration
        };

        _dataManager.AddMedicine(medicine);

        if (VisualRoot is MainWindow main && main.DataContext is MainWindowViewModel vm)
            vm.RefreshAll();

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
        stack.Children.Add(new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap });

        var btn = new Button { Content = "OK", Width = 100, Height = 35 };
        btn.Click += (_, _) => dialog.Close();

        stack.Children.Add(btn);
        dialog.Content = stack;

        await dialog.ShowDialog(this);
    }
}