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
        // ====================== ВАЛИДАЦИЯ ======================

        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            ShowError("Название лекарства обязательно для заполнения!");
            NameBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(CategoryBox.Text))
        {
            ShowError("Категория лекарства обязательна!");
            CategoryBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(ManufacturerBox.Text))
        {
            ShowError("Укажите производителя!");
            ManufacturerBox.Focus();
            return;
        }

        if (ManufactureDatePicker.SelectedDate == null)
        {
            ShowError("Выберите дату производства!");
            return;
        }

        if (ExpirationDatePicker.SelectedDate == null)
        {
            ShowError("Выберите срок годности!");
            return;
        }

        DateTime manufacture = ManufactureDatePicker.SelectedDate.Value.DateTime.Date;
        DateTime expiration = ExpirationDatePicker.SelectedDate.Value.DateTime.Date;

        if (manufacture > DateTime.Now.Date)
        {
            ShowError("Дата производства не может быть в будущем!");
            return;
        }

        if (expiration <= manufacture)
        {
            ShowError("Срок годности должен быть позже даты производства!");
            return;
        }

        if (string.IsNullOrWhiteSpace(RegNumberBox.Text))
        {
            ShowError("Регистрационный номер Минздрава обязателен!");
            RegNumberBox.Focus();
            return;
        }

        // ====================== Создание объекта ======================
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

    private void ShowError(string message)
    {
        var msgBox = new Window
        {
            Title = "Ошибка ввода",
            Width = 400,
            Height = 180,
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