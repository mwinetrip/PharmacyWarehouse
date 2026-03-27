using System.Collections.ObjectModel;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public class SalesInvoicesViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public ObservableCollection<SalesInvoice> SalesInvoices => _dataManager.SalesInvoices;

    public SalesInvoicesViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
    }
}