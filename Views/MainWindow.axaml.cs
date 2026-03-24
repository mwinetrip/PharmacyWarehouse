using Avalonia.Controls;

namespace PharmacyWarehouse.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void AddMedicine_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var window = new AddMedicineWindow();
        window.ShowDialog(this);
    }
    
}