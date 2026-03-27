using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PharmacyWarehouse.Views;

public partial class SalesInvoicesView : UserControl
{
    public SalesInvoicesView()
    {
        InitializeComponent();
    }

    private void AddSales_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddSalesInvoiceWindow();
        window.ShowDialog((Window)this.VisualRoot!);
    }
}