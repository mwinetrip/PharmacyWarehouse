using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.ViewModels.Pages;

namespace PharmacyWarehouse.Views;

public partial class SalesInvoicesView : UserControl
{
    public SalesInvoicesView()
    {
        InitializeComponent();
    }

    private async void AddSales_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddSalesInvoiceWindow();
        await window.ShowDialog((Window)VisualRoot!);
        ForceRefresh();
    }

    private async void EditSales_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not SalesInvoicesViewModel vm || vm.SelectedSales == null) return;
        var window = new AddSalesInvoiceWindow(vm.SelectedSales);
        await window.ShowDialog((Window)VisualRoot!);
        ForceRefresh();
    }

    private void ForceRefresh()
    {
        if (DataContext is not SalesInvoicesViewModel vm) return;
        var grid = this.FindControl<DataGrid>("MainGrid");
        if (grid == null) return;
        grid.ItemsSource = null;
        grid.ItemsSource = vm.SalesInvoices;
    }
}