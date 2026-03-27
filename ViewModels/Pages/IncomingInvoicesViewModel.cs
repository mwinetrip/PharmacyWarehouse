using System.Collections.ObjectModel;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public class IncomingInvoicesViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public ObservableCollection<IncomingInvoice> IncomingInvoices => _dataManager.IncomingInvoices;

    public IncomingInvoicesViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
    }
}