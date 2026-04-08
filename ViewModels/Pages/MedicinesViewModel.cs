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

    public ObservableCollection<Medicine> Medicines => _dataManager.Medicines;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private Medicine? selectedMedicine;

    public IRelayCommand DeleteCommand { get; }

    public MedicinesViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
        DeleteCommand = new RelayCommand(DeleteSelected, CanDelete);
    }

    private void DeleteSelected()
    {
        if (SelectedMedicine == null) return;

        bool isUsed = _dataManager.IncomingInvoices.Any(i => i.Items.Any(item => item.MedicineId == SelectedMedicine.Id)) ||
                      _dataManager.SalesInvoices.Any(i => i.Items.Any(item => item.MedicineId == SelectedMedicine.Id));

        if (isUsed)
        {
            MessageBoxService.ShowErrorAsync(null, "Невозможно удалить", 
                "Лекарство используется в накладных и не может быть удалено.");
            return;
        }

        _dataManager.Medicines.Remove(SelectedMedicine);
        _dataManager.SaveAll();
        Refresh();
    }

    private bool CanDelete() => SelectedMedicine != null;

    public void Refresh()
    {
        OnPropertyChanged(nameof(Medicines));
    }
}