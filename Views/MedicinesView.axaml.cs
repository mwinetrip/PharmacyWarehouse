using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Views.AddWindow;

namespace PharmacyWarehouse.Views;

public partial class MedicinesView : UserControl
{
    public MedicinesView()
    {
        InitializeComponent();
    }

    private void AddMedicine_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddMedicineWindow();
        window.ShowDialog((Window)this.VisualRoot!);
    }
}