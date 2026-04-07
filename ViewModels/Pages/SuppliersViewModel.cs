using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public partial class SuppliersViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public ObservableCollection<Supplier> Suppliers => _dataManager.Suppliers;

    [ObservableProperty]
    private Supplier? selectedSupplier;

    public IRelayCommand DeleteCommand { get; }

    public SuppliersViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
        DeleteCommand = new RelayCommand(DeleteSelected, CanDelete);
    }

    private void DeleteSelected()
    {
        if (SelectedSupplier == null) return;

        bool isUsed = _dataManager.IncomingInvoices.Any(i => i.SupplierId == SelectedSupplier.Id);

        if (isUsed)
        {
            MessageBoxService.ShowErrorAsync(null, "Невозможно удалить", 
                "Поставщик используется в приходных накладных.");
            return;
        }

        _dataManager.Suppliers.Remove(SelectedSupplier);
        _dataManager.SaveAll();
        Refresh();
    }

    private bool CanDelete() => SelectedSupplier != null;

    public void Refresh()
    {
        OnPropertyChanged(nameof(Suppliers));
    }
}