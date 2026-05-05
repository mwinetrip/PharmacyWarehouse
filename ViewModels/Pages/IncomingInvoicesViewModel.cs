using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public partial class IncomingInvoicesViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand), nameof(EditCommand))]
    private IncomingInvoice? selectedIncoming;

    public IncomingInvoicesViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
        DeleteCommand = new RelayCommand(DeleteSelected, () => SelectedIncoming != null);
        EditCommand = new RelayCommand(() => { }, () => SelectedIncoming != null);
    }

    public ObservableCollection<IncomingInvoice> IncomingInvoices => _dataManager.IncomingInvoices;

    public IRelayCommand DeleteCommand { get; }
    public IRelayCommand EditCommand { get; }

    public void Refresh() => OnPropertyChanged(nameof(IncomingInvoices));

    private void DeleteSelected()
    {
        if (SelectedIncoming == null) return;
        _dataManager.IncomingInvoices.Remove(SelectedIncoming);
        _dataManager.SaveAll();
        Refresh();
    }
}
