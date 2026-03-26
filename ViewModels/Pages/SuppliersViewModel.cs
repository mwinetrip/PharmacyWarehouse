using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public class SuppliersViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public SuppliersViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
    }
}