using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public partial class SalesInvoicesViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public ObservableCollection<SalesInvoice> SalesInvoices => _dataManager.SalesInvoices;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private SalesInvoice? selectedSales;

    public IRelayCommand DeleteCommand { get; }

    public SalesInvoicesViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
        DeleteCommand = new RelayCommand(DeleteSelected, CanDelete);
    }

    private void DeleteSelected()
    {
        if (SelectedSales == null) return;

        _dataManager.SalesInvoices.Remove(SelectedSales);
        _dataManager.SaveAll();
        Refresh();
    }

    private bool CanDelete() => SelectedSales != null;

    public void Refresh()
    {
        OnPropertyChanged(nameof(SalesInvoices));
    }
}