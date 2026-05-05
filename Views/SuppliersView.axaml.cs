using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.ViewModels.Pages;

namespace PharmacyWarehouse.Views;

public partial class SuppliersView : UserControl
{
    public SuppliersView()
    {
        InitializeComponent();
    }

    private async void AddSupplier_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddSupplierWindow();
        await window.ShowDialog((Window)VisualRoot!);
        ForceRefresh();
    }

    private async void EditSupplier_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not SuppliersViewModel vm || vm.SelectedSupplier == null) return;
        var window = new AddSupplierWindow(vm.SelectedSupplier);
        await window.ShowDialog((Window)VisualRoot!);
        ForceRefresh();
    }

    private void ForceRefresh()
    {
        if (DataContext is not SuppliersViewModel vm) return;
        var grid = this.FindControl<DataGrid>("MainGrid");
        if (grid == null) return;
        grid.ItemsSource = null;
        grid.ItemsSource = vm.Suppliers;
    }
}