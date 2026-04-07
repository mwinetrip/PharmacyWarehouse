using System.Collections.ObjectModel;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public class SuppliersViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public ObservableCollection<Supplier> Suppliers => _dataManager.Suppliers;

    public SuppliersViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
    }
    
    public void Refresh()
    {
        OnPropertyChanged(nameof(Suppliers));
    }
}