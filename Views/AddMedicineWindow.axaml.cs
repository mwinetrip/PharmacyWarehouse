using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.Views;

public partial class AddMedicineWindow : Window
{
    private readonly DataManager _dataManager;
    private readonly bool _isEditMode;
    private readonly Medicine? _medicineToEdit;

    public AddMedicineWindow(Medicine? medicineToEdit = null)
    {
        InitializeComponent();
        _dataManager = DataManager.Instance;
        _medicineToEdit = medicineToEdit;
        _isEditMode = medicineToEdit != null;

        Title = _isEditMode ? "Редактирование лекарства" : "Добавление нового лекарства";

        if (_isEditMode && _medicineToEdit != null)
            LoadMedicineData(_medicineToEdit);
        else
        {
            ManufactureDatePicker.SelectedDate = DateTime.Now.AddMonths(-3);
            ExpirationDatePicker.SelectedDate = DateTime.Now.AddYears(1);
        }
    }

    private void LoadMedicineData(Medicine medicine)
    {
        NameBox.Text = medicine.Name;
        CategoryBox.Text = medicine.Category;
        ManufacturerBox.Text = medicine.Manufacturer;
        PackageBox.Text = medicine.PackageType;
        RegNumberBox.Text = medicine.RegistrationNumber;
        ManufactureDatePicker.SelectedDate = medicine.ManufactureDate;
        ExpirationDatePicker.SelectedDate = medicine.ExpirationDate;
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            await DialogHelper.ShowErrorAsync("Название лекарства обязательно!", this);
            NameBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(CategoryBox.Text))
        {
            await DialogHelper.ShowErrorAsync("Категория лекарства обязательна!", this);
            CategoryBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(ManufacturerBox.Text))
        {
            await DialogHelper.ShowErrorAsync("Укажите производителя!", this);
            ManufacturerBox.Focus();
            return;
        }

        if (ManufactureDatePicker.SelectedDate == null)
        {
            await DialogHelper.ShowErrorAsync("Выберите дату производства!", this);
            return;
        }

        if (ExpirationDatePicker.SelectedDate == null)
        {
            await DialogHelper.ShowErrorAsync("Выберите срок годности!", this);
            return;
        }

        var manufacture = ManufactureDatePicker.SelectedDate.Value.DateTime.Date;
        var expiration = ExpirationDatePicker.SelectedDate.Value.DateTime.Date;

        if (manufacture > DateTime.Now.Date)
        {
            await DialogHelper.ShowErrorAsync("Дата производства не может быть в будущем!", this);
            return;
        }

        if (expiration <= manufacture)
        {
            await DialogHelper.ShowErrorAsync("Срок годности должен быть позже даты производства!", this);
            return;
        }

        if (string.IsNullOrWhiteSpace(RegNumberBox.Text))
        {
            await DialogHelper.ShowErrorAsync("Регистрационный номер Минздрава обязателен!", this);
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

        if (_isEditMode && _medicineToEdit != null)
        {
            medicine.Id = _medicineToEdit.Id;
            _dataManager.UpdateMedicine(medicine);
        }
        else
        {
            _dataManager.AddMedicine(medicine);
        }

        Close();
    }
}
