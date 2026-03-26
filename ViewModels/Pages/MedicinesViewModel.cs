using System.Collections.ObjectModel;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;

namespace PharmacyWarehouse.ViewModels.Pages;

public class MedicinesViewModel : ViewModelBase
{
    private readonly DataManager _dataManager;

    public ObservableCollection<Medicine> Medicines => _dataManager.Medicines;

    public MedicinesViewModel(DataManager dataManager)
    {
        _dataManager = dataManager;
    }

    // Метод для обновления (пока не нужен, но пригодится)
    public void Refresh() { }
}