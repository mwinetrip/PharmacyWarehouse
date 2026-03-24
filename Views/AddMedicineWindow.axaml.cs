using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.Views;

public partial class AddMedicineWindow : Window
{
    private readonly DataManager _dataManager;

    public AddMedicineWindow()
    {
        InitializeComponent();
        _dataManager = new DataManager();

        // Устанавливаем даты по умолчанию
        ManufactureDatePicker.SelectedDate = DateTime.Now.AddMonths(-1);
        ExpirationDatePicker.SelectedDate = DateTime.Now.AddYears(2);
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        // Простая проверка на заполненность (валидацию сделаем позже)
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            NameBox.Focus();
            return;
        }

        var medicine = new Medicine
        {
            Name = NameBox.Text.Trim(),
            Category = CategoryBox.Text?.Trim() ?? "",
            Manufacturer = ManufacturerBox.Text?.Trim() ?? "",
            PackageType = PackageBox.Text?.Trim() ?? "",
            RegistrationNumber = RegNumberBox.Text?.Trim() ?? "",
            ManufactureDate = ManufactureDatePicker.SelectedDate?.DateTime ?? DateTime.Now,
            ExpirationDate = ExpirationDatePicker.SelectedDate?.DateTime ?? DateTime.Now.AddYears(1)
        };

        _dataManager.AddMedicine(medicine);

        Close();
    }
}