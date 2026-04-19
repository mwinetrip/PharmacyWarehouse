using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Views.AddWindow;

namespace PharmacyWarehouse.Views;

public partial class CustomersView : UserControl
{
    public CustomersView()
    {
        InitializeComponent();
    }

    private void AddCustomer_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddCustomerWindow();
        window.ShowDialog((Window)this.VisualRoot!);
    }
}