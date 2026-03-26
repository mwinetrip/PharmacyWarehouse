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

        // Значения по умолчанию
        ManufactureDatePicker.SelectedDate = DateTime.Now.AddMonths(-3);
        ExpirationDatePicker.SelectedDate = DateTime.Now.AddYears(1);
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        // === Проверки корректности ввода (валидация) ===

        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            NameBox.Focus();
            // Здесь позже можно показать MessageBox с ошибкой
            return;
        }

        if (string.IsNullOrWhiteSpace(CategoryBox.Text))
        {
            CategoryBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(ManufacturerBox.Text))
        {
            ManufacturerBox.Focus();
            return;
        }

        if (ManufactureDatePicker.SelectedDate == null)
        {
            // ошибка
            return;
        }

        if (ExpirationDatePicker.SelectedDate == null)
        {
            // ошибка
            return;
        }

        DateTime manufacture = ManufactureDatePicker.SelectedDate.Value.DateTime;
        DateTime expiration = ExpirationDatePicker.SelectedDate.Value.DateTime;

        if (manufacture > DateTime.Now)
        {
            // Дата производства не может быть в будущем
            return;
        }

        if (expiration <= manufacture)
        {
            // Срок годности должен быть позже даты производства
            return;
        }

        if (string.IsNullOrWhiteSpace(RegNumberBox.Text))
        {
            RegNumberBox.Focus();
            return;
        }

        // Если все проверки прошли — создаём объект
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