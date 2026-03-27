using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PharmacyWarehouse.Views;

public partial class IncomingInvoicesView : UserControl
{
    public IncomingInvoicesView()
    {
        InitializeComponent();
    }

    private void AddIncomingInvoice_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddIncomingInvoiceWindow();
        window.ShowDialog((Window)this.VisualRoot!);
    }
}