using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System;

namespace PharmacyWarehouse.Views;

public partial class AddMedicineWindow : Window
{
    private readonly DataManager _dataManager;

    public AddMedicineWindow()
    {
        InitializeComponent();
        _dataManager = new DataManager();

        ManufactureDatePicker.SelectedDate = DateTime.Now.AddMonths(-3);
        ExpirationDatePicker.SelectedDate = DateTime.Now.AddYears(1);
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Название лекарства обязательно для заполнения!");
            NameBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(CategoryBox.Text))
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Категория лекарства обязательна!");
            CategoryBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(ManufacturerBox.Text))
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Укажите производителя!");
            ManufacturerBox.Focus();
            return;
        }

        if (ManufactureDatePicker.SelectedDate == null)
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Выберите дату производства!");
            return;
        }

        if (ExpirationDatePicker.SelectedDate == null)
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Выберите срок годности!");
            return;
        }

        DateTime manufacture = ManufactureDatePicker.SelectedDate.Value.DateTime.Date;
        DateTime expiration = ExpirationDatePicker.SelectedDate.Value.DateTime.Date;

        if (manufacture > DateTime.Now.Date)
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Дата производства не может быть в будущем!");
            return;
        }

        if (expiration <= manufacture)
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Срок годности должен быть позже даты производства!");
            return;
        }

        if (string.IsNullOrWhiteSpace(RegNumberBox.Text))
        {
            await MessageBoxService.ShowErrorAsync(this, "Ошибка", "Регистрационный номер Минздрава обязателен!");
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
        Close();
    }
}