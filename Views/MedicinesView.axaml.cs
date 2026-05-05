using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.ViewModels.Pages;

namespace PharmacyWarehouse.Views;

public partial class MedicinesView : UserControl
{
    public MedicinesView()
    {
        InitializeComponent();
    }

    private async void AddMedicine_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddMedicineWindow();
        await window.ShowDialog((Window)VisualRoot!);
        ForceRefresh();
    }

    private async void EditMedicine_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MedicinesViewModel vm || vm.SelectedMedicine == null) return;
        var window = new AddMedicineWindow(vm.SelectedMedicine);
        await window.ShowDialog((Window)VisualRoot!);
        ForceRefresh();
    }

    private void ForceRefresh()
    {
        if (DataContext is not MedicinesViewModel vm) return;
        var grid = this.FindControl<DataGrid>("MainGrid");
        if (grid == null) return;
        grid.ItemsSource = null;
        grid.ItemsSource = vm.Medicines;
    }
}