using System;
using System.Collections.ObjectModel;
using PharmacyWarehouse.Models;

namespace PharmacyWarehouse.ViewModels;

public class MedicinesViewModel
{
    public ObservableCollection<Medicine> Medicines { get; set; }

    public MedicinesViewModel()
    {
        Medicines = new ObservableCollection<Medicine>();
    }

    public void AddTestMedicine()
    {
        Medicines.Add(new Medicine
        {
            Name = "Парацетамол",
            Category = "Жаропонижающее",
            Manufacturer = "ФармЗавод",
            PackageType = "Таблетки",
            RegistrationNumber = "REG-001",
            ManufactureDate = DateTime.Now.AddMonths(-2),
            ExpirationDate = DateTime.Now.AddYears(1)
        });
    }
}