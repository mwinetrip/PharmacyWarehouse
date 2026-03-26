using System;
using PharmacyWarehouse.Services;
using PharmacyWarehouse.ViewModels.Pages;   // создадим эту папку чуть позже

namespace PharmacyWarehouse.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    // ViewModels для каждой вкладки
    public MedicinesViewModel MedicinesVM { get; }
    public SuppliersViewModel SuppliersVM { get; }
    public CustomersViewModel CustomersVM { get; }
    public IncomingInvoicesViewModel IncomingVM { get; }
    public SalesInvoicesViewModel SalesVM { get; }

    public MainWindowViewModel()
    {
        _dataManager = new DataManager();

        MedicinesVM = new MedicinesViewModel(_dataManager);
        SuppliersVM = new SuppliersViewModel(_dataManager);
        CustomersVM = new CustomersViewModel(_dataManager);
        IncomingVM = new IncomingInvoicesViewModel(_dataManager);
        SalesVM = new SalesInvoicesViewModel(_dataManager);
    }
}