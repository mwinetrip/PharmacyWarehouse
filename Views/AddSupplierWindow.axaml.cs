using Avalonia.Controls;
using Avalonia.Interactivity;
using PharmacyWarehouse.Models;
using PharmacyWarehouse.Services;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using PharmacyWarehouse.ViewModels;

namespace PharmacyWarehouse.Views;

public partial class AddSupplierWindow : Window
{
    private readonly DataManager _dataManager;

    public AddSupplierWindow()
    {
        InitializeComponent();
        _dataManager = DataManager.Instance;
    }

    private async void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text))
        {
            await ShowErrorAsync("Название поставщика обязательно!");
            NameBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(InnBox.Text) || !IsValidInn(InnBox.Text))
        {
            await ShowErrorAsync("ИНН должен содержать 10 или 12 цифр!");
            InnBox.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(PhoneBox.Text))
        {
            await ShowErrorAsync("Укажите телефон!");
            PhoneBox.Focus();
            return;
        }

        var supplier = new Supplier
        {
            Name = NameBox.Text.Trim(),
            Inn = InnBox.Text.Trim(),
            Address = AddressBox.Text?.Trim() ?? "",
            Phone = PhoneBox.Text.Trim(),
            Bank = BankBox.Text?.Trim() ?? "",
            AccountNumber = AccountBox.Text?.Trim() ?? ""
        };

        _dataManager.AddSupplier(supplier);

        if (VisualRoot is MainWindow main && main.DataContext is MainWindowViewModel vm)
            vm.RefreshAll();

        Close();
    }

    private bool IsValidInn(string inn)
    {
        if (string.IsNullOrWhiteSpace(inn)) return false;
        inn = inn.Trim();
        return (inn.Length == 10 || inn.Length == 12) && long.TryParse(inn, out _);
    }

    private async Task ShowErrorAsync(string message)
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

        await dialog.ShowDialog(this);
    }
}