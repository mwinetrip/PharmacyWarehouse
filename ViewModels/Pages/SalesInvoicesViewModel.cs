using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public class SalesInvoicesViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public SalesInvoicesViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
    }
}