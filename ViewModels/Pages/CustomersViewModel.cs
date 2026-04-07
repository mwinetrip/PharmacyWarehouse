using System.Collections.ObjectModel;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public class CustomersViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;
    
    public ObservableCollection<Customer> Customers => _dataManager.Customers;
    
    public CustomersViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
    }
    
    public void Refresh()
    {
        OnPropertyChanged(nameof(Customers));
    }
}