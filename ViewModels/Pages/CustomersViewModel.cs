using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public class CustomersViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public CustomersViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
    }
}