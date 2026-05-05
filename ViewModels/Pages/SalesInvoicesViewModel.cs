using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public partial class SalesInvoicesViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand), nameof(EditCommand))]
    private SalesInvoice? selectedSales;

    public SalesInvoicesViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
        DeleteCommand = new RelayCommand(DeleteSelected, () => SelectedSales != null);
        EditCommand = new RelayCommand(() => { }, () => SelectedSales != null);
    }

    public ObservableCollection<SalesInvoice> SalesInvoices => _dataManager.SalesInvoices;

    public IRelayCommand DeleteCommand { get; }
    public IRelayCommand EditCommand { get; }

    public void Refresh() => OnPropertyChanged(nameof(SalesInvoices));

    private void DeleteSelected()
    {
        if (SelectedSales == null) return;
        _dataManager.SalesInvoices.Remove(SelectedSales);
        _dataManager.SaveAll();
        Refresh();
    }
}
