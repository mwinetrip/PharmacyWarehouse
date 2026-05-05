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

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand), nameof(EditCommand))]
    private Customer? selectedCustomer;

    public CustomersViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
        DeleteCommand = new RelayCommand(DeleteSelected, () => SelectedCustomer != null);
        EditCommand = new RelayCommand(() => { }, () => SelectedCustomer != null);
    }

    public ObservableCollection<Customer> Customers => _dataManager.Customers;

    public IRelayCommand DeleteCommand { get; }
    public IRelayCommand EditCommand { get; }

    public void Refresh() => OnPropertyChanged(nameof(Customers));

    private void DeleteSelected()
    {
        if (SelectedCustomer == null) return;

        if (_dataManager.SalesInvoices.Any(i => i.CustomerId == SelectedCustomer.Id))
        {
            _ = DialogHelper.ShowErrorAsync("Покупатель используется в счетах-фактурах и не может быть удалён.");
            return;
        }

        _dataManager.Customers.Remove(SelectedCustomer);
        _dataManager.SaveAll();
        Refresh();
    }
}
