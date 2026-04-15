using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
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
            ShowError("Лекарство используется в накладных и не может быть удалено.");
            return;
        }

        _dataManager.Medicines.Remove(SelectedMedicine);
        _dataManager.SaveAll();
        Refresh();
    }

    private bool CanDelete() => SelectedMedicine != null;

    public void Refresh() => OnPropertyChanged(nameof(Medicines));

    private async void ShowError(string message)
    {
        var dialog = new Window
        {
            Title = "Ошибка",
            Width = 420,
            Height = 170,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var stack = new StackPanel { Margin = new Thickness(25), Spacing = 20 };
        stack.Children.Add(new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap });

        var btn = new Button { Content = "OK", Width = 100, Height = 35 };
        btn.Click += (_, _) => dialog.Close();

        stack.Children.Add(btn);
        dialog.Content = stack;

        await dialog.ShowDialog(null);
    }
}