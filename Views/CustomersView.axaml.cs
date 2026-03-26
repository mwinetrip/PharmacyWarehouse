using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PharmacyWarehouse.Views;

public partial class CustomersView : UserControl
{
    public CustomersView()
    {
        InitializeComponent();
    }

    private void AddCustomer_Click(object? sender, RoutedEventArgs e)
    {
        // Пока заглушка — позже создадим окно
        // var window = new AddCustomerWindow();
        // window.ShowDialog((Window)this.VisualRoot!);
    }
}