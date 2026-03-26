using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PharmacyWarehouse.Views;

public partial class IncomingInvoicesView : UserControl
{
    public IncomingInvoicesView()
    {
        InitializeComponent();
    }

    private void AddIncomingInvoices_Click(object? sender, RoutedEventArgs e)
    {
        // Пока заглушка — позже создадим окно
        // var window = new AddIncomingInvoicesWindow();
        // window.ShowDialog((Window)this.VisualRoot!);
    }
}