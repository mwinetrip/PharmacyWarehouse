using Avalonia.Controls;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.Views;

public partial class AddMedicineWindow : Window
{
    private DataManager dataManager;

    public AddMedicineWindow()
    {
        InitializeComponent();
        dataManager = new DataManager();
    }

    private void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var medicine = new Medicine
        {
            Id = dataManager.Medicines.Count + 1,
            Name = NameBox.Text,
            Category = CategoryBox.Text,
            Manufacturer = ManufacturerBox.Text,
            PackageType = PackageBox.Text
        };

        dataManager.Medicines.Add(medicine);
        dataManager.SaveAll();

        Close();
    }
}