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

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand), nameof(EditCommand))]
    private Supplier? selectedSupplier;

    public SuppliersViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
        DeleteCommand = new RelayCommand(DeleteSelected, () => SelectedSupplier != null);
        EditCommand = new RelayCommand(() => { }, () => SelectedSupplier != null);
    }

    public ObservableCollection<Supplier> Suppliers => _dataManager.Suppliers;

    public IRelayCommand DeleteCommand { get; }
    public IRelayCommand EditCommand { get; }

    public void Refresh() => OnPropertyChanged(nameof(Suppliers));

    private void DeleteSelected()
    {
        if (SelectedSupplier == null) return;

        if (_dataManager.IncomingInvoices.Any(i => i.SupplierId == SelectedSupplier.Id))
        {
            _ = DialogHelper.ShowErrorAsync("Поставщик используется в приходных накладных и не может быть удалён.");
            return;
        }

        _dataManager.Suppliers.Remove(SelectedSupplier);
        _dataManager.SaveAll();
        Refresh();
    }
}
