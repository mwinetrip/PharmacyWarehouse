using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public partial class MedicinesViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand), nameof(EditCommand))]
    private Medicine? selectedMedicine;

    public MedicinesViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
        DeleteCommand = new RelayCommand(DeleteSelected, () => SelectedMedicine != null);
        EditCommand = new RelayCommand(() => { }, () => SelectedMedicine != null);
    }

    public ObservableCollection<Medicine> Medicines => _dataManager.Medicines;

    public IRelayCommand DeleteCommand { get; }
    public IRelayCommand EditCommand { get; }

    public void Refresh() => OnPropertyChanged(nameof(Medicines));

    private void DeleteSelected()
    {
        if (SelectedMedicine == null) return;

        var usedInIncoming = _dataManager.IncomingInvoices.Any(i => i.Items.Any(item => item.MedicineId == SelectedMedicine.Id));
        var usedInSales = _dataManager.SalesInvoices.Any(i => i.Items.Any(item => item.MedicineId == SelectedMedicine.Id));

        if (usedInIncoming || usedInSales)
        {
            _ = DialogHelper.ShowErrorAsync("Лекарство используется в накладных и не может быть удалено.");
            return;
        }

        _dataManager.Medicines.Remove(SelectedMedicine);
        _dataManager.SaveAll();
        Refresh();
    }
}
