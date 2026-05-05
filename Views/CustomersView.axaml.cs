using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.ViewModels.Pages;

namespace PharmacyWarehouse.Views;

public partial class CustomersView : UserControl
{
    public CustomersView()
    {
        InitializeComponent();
    }

    private async void AddCustomer_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddCustomerWindow();
        await window.ShowDialog((Window)VisualRoot!);
        ForceRefresh();
    }

    private async void EditCustomer_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not CustomersViewModel vm || vm.SelectedCustomer == null) return;
        var window = new AddCustomerWindow(vm.SelectedCustomer);
        await window.ShowDialog((Window)VisualRoot!);
        ForceRefresh();
    }

    private void ForceRefresh()
    {
        if (DataContext is not CustomersViewModel vm) return;
        var grid = this.FindControl<DataGrid>("MainGrid");
        if (grid == null) return;
        grid.ItemsSource = null;
        grid.ItemsSource = vm.Customers;
    }
}