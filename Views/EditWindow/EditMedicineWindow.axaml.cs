using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;

namespace PharmacyWarehouse.Views.EditWindow;

public partial class EditMedicineWindow : Window
{
    private readonly DataManager _dataManager;
    private readonly Medicine _originalMedicine;

    public EditMedicineWindow(Medicine medicineToEdit)
    {
        InitializeComponent();
        _dataManager = DataManager.Instance;
        _originalMedicine = medicineToEdit;

        // Заполняем поля текущими данными
        NameBox.Text = medicineToEdit.Name;
        CategoryBox.Text = medicineToEdit.Category;
        ManufacturerBox.Text = medicineToEdit.Manufacturer;
        PackageBox.Text = medicineToEdit.PackageType;
        RegNumberBox.Text = medicineToEdit.RegistrationNumber;
        ManufactureDatePicker.SelectedDate = medicineToEdit.ManufactureDate;
        ExpirationDatePicker.SelectedDate = medicineToEdit.ExpirationDate;
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            await ShowErrorAsync("Название лекарства обязательно!");
            return;
        }

        // Обновляем оригинальный объект
        _originalMedicine.Name = NameBox.Text.Trim();
        _originalMedicine.Category = CategoryBox.Text.Trim();
        _originalMedicine.Manufacturer = ManufacturerBox.Text.Trim();
        _originalMedicine.PackageType = PackageBox.Text?.Trim() ?? "";
        _originalMedicine.RegistrationNumber = RegNumberBox.Text.Trim();
        _originalMedicine.ManufactureDate = ManufactureDatePicker.SelectedDate?.DateTime.Date ?? DateTime.Now;
        _originalMedicine.ExpirationDate = ExpirationDatePicker.SelectedDate?.DateTime.Date ?? DateTime.Now;

        _dataManager.SaveAll();

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