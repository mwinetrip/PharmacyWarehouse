using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PharmacyWarehouse.Views;

public partial class SalesInvoicesView : UserControl
{
    public SalesInvoicesView()
    {
        InitializeComponent();
    }

    private void AddSalesInvoices_Click(object? sender, RoutedEventArgs e)
    {
        // Пока заглушка — позже создадим окно
        // var window = new AddSalesInvoicesWindow();
        // window.ShowDialog((Window)this.VisualRoot!);
    }
}