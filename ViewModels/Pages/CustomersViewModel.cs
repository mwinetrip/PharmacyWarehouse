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

public partial class CustomersViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public ObservableCollection<Customer> Customers => _dataManager.Customers;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private Customer? selectedCustomer;

    public IRelayCommand DeleteCommand { get; }

    public CustomersViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
        DeleteCommand = new RelayCommand(DeleteSelected, CanDelete);
    }

    private void DeleteSelected()
    {
        if (SelectedCustomer == null) return;

        bool isUsed = _dataManager.SalesInvoices.Any(i => i.CustomerId == SelectedCustomer.Id);

        if (isUsed)
        {
            ShowError("Покупатель используется в счетах-фактурах и не может быть удалён.");
            return;
        }

        _dataManager.Customers.Remove(SelectedCustomer);
        _dataManager.SaveAll();
        Refresh();
    }

    private bool CanDelete() => SelectedCustomer != null;

    public void Refresh() => OnPropertyChanged(nameof(Customers));

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