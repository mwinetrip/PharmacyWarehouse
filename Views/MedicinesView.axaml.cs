using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Views;   // для AddMedicineWindow
using PharmacyWarehouse.ViewModels.Pages;

namespace PharmacyWarehouse.Views;

public partial class MedicinesView : UserControl
{
    public MedicinesView()
    {
        InitializeComponent();
        DataContext = new MedicinesViewModel(new Services.DataManager()); // временно, позже поправим через MainWindowVM
    }

    private void AddMedicine_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddMedicineWindow();
        window.ShowDialog((Window)this.VisualRoot!);
    }
}