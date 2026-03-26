using Avalonia.Controls;

namespace PharmacyWarehouse.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // Можно добавить логику при закрытии окна (сохранение данных)
        this.Closed += (s, e) =>
        {
            // При желании можно вызвать DataManager.SaveAll() здесь
        };
    }
}