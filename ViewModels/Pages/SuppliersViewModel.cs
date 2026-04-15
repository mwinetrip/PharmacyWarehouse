using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public partial class SuppliersViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public ObservableCollection<Supplier> Suppliers => _dataManager.Suppliers;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private Supplier? selectedSupplier;

    public IRelayCommand DeleteCommand { get; }

    public SuppliersViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
        DeleteCommand = new RelayCommand(DeleteSelected, CanDelete);
    }

    private void DeleteSelected()
    {
        if (SelectedSupplier == null) return;

        bool isUsed = _dataManager.IncomingInvoices.Any(i => i.SupplierId == SelectedSupplier.Id);

        if (isUsed)
        {
            ShowError("Поставщик используется в приходных накладных и не может быть удалён.");
            return;
        }

        _dataManager.Suppliers.Remove(SelectedSupplier);
        _dataManager.SaveAll();
        Refresh();
    }

    private bool CanDelete() => SelectedSupplier != null;

    public void Refresh() => OnPropertyChanged(nameof(Suppliers));

    private async void ShowError(string message)
    {
        var dialog = new Window
        {
            Title = "Ошибка",
            Width = 420,
            Height = 170,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var stack = new StackPanel { Margin = new Thickness(25), Spacing = 20 };
        stack.Children.Add(new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap });

        var btn = new Button { Content = "OK", Width = 100, Height = 35 };
        btn.Click += (_, _) => dialog.Close();

        stack.Children.Add(btn);
        dialog.Content = stack;

        await dialog.ShowDialog(null);
    }
}