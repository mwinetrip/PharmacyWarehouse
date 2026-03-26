using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PharmacyWarehouse.Views;

public partial class SuppliersView : UserControl
{
    public SuppliersView()
    {
        InitializeComponent();
    }

    private void AddSupplier_Click(object? sender, RoutedEventArgs e)
    {
        // Пока заглушка — позже создадим окно
        // var window = new AddSupplierWindow();
        // window.ShowDialog((Window)this.VisualRoot!);
    }
}