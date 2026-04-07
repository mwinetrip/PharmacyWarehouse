using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public partial class IncomingInvoicesViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public ObservableCollection<IncomingInvoice> IncomingInvoices => _dataManager.IncomingInvoices;

    [ObservableProperty]
    private IncomingInvoice? selectedIncoming;

    public IRelayCommand DeleteCommand { get; }

    public IncomingInvoicesViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
        DeleteCommand = new RelayCommand(DeleteSelected, CanDelete);
    }

    private void DeleteSelected()
    {
        if (SelectedIncoming == null) return;

        _dataManager.IncomingInvoices.Remove(SelectedIncoming);
        _dataManager.SaveAll();
        Refresh();
    }

    private bool CanDelete() => SelectedIncoming != null;

    public void Refresh()
    {
        OnPropertyChanged(nameof(IncomingInvoices));
    }
}