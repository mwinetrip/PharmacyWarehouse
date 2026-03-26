using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public class IncomingInvoicesViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public IncomingInvoicesViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
    }
}