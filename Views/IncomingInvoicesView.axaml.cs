using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Views.AddWindow;

namespace PharmacyWarehouse.Views;

public partial class IncomingInvoicesView : UserControl
{
    public IncomingInvoicesView()
    {
        InitializeComponent();
    }

    private void AddIncoming_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddIncomingInvoiceWindow();
        window.ShowDialog((Window)this.VisualRoot!);
    }
}