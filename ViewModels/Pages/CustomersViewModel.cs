using System.Collections.ObjectModel;
using System.Linq;
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
            MessageBoxService.ShowErrorAsync(null, "Невозможно удалить", 
                "Покупатель используется в счетах-фактурах и не может быть удалён.");
            return;
        }

        _dataManager.Customers.Remove(SelectedCustomer);
        _dataManager.SaveAll();
        Refresh();
    }

    private bool CanDelete() => SelectedCustomer != null;

    public void Refresh()
    {
        OnPropertyChanged(nameof(Customers));
    }
}