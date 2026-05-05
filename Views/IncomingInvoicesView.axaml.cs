using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.ViewModels.Pages;

namespace PharmacyWarehouse.Views;

public partial class IncomingInvoicesView : UserControl
{
    public IncomingInvoicesView()
    {
        InitializeComponent();
    }

    private async void AddIncoming_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddIncomingInvoiceWindow();
        await window.ShowDialog((Window)VisualRoot!);
        ForceRefresh();
    }

    private async void EditIncoming_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not IncomingInvoicesViewModel vm || vm.SelectedIncoming == null) return;
        var window = new AddIncomingInvoiceWindow(vm.SelectedIncoming);
        await window.ShowDialog((Window)VisualRoot!);
        ForceRefresh();
    }

    private void ForceRefresh()
    {
        if (DataContext is not IncomingInvoicesViewModel vm) return;
        var grid = this.FindControl<DataGrid>("MainGrid");
        if (grid == null) return;
        grid.ItemsSource = null;
        grid.ItemsSource = vm.IncomingInvoices;
    }
}