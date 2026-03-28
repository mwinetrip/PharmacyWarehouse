using Avalonia.Controls;
using System.Threading.Tasks;

namespace PharmacyWarehouse.Services;

/// <summary>
/// Сервис для показа красивых диалоговых окон (MessageBox)
/// </summary>
public static class MessageBoxService
{
    /// <summary>
    /// Показать ошибку
    /// </summary>
    public static async Task ShowErrorAsync(Window owner, string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 420,
            Height = 180,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var content = new StackPanel { Margin = new Avalonia.Thickness(20), Spacing = 10 };
        content.Children.Add(new TextBlock 
        { 
            Text = message, 
            TextWrapping = Avalonia.Media.TextWrapping.Wrap 
        });

        var okButton = new Button 
        { 
            Content = "OK", 
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            Width = 100,
            Height = 35
        };
        okButton.Click += (s, e) => dialog.Close();

        content.Children.Add(okButton);
        dialog.Content = content;

        await dialog.ShowDialog(owner);
    }

    /// <summary>
    /// Показать предупреждение с выбором Да / Отмена
    /// </summary>
    public static async Task<bool> ShowWarningAsync(Window owner, string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 460,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var content = new StackPanel { Margin = new Avalonia.Thickness(20), Spacing = 15 };

        content.Children.Add(new TextBlock 
        { 
            Text = message, 
            TextWrapping = Avalonia.Media.TextWrapping.Wrap 
        });

        var buttonPanel = new StackPanel 
        { 
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            Spacing = 15
        };

        var yesButton = new Button { Content = "Да", Width = 100, Height = 35 };
        var cancelButton = new Button { Content = "Отмена", Width = 100, Height = 35 };

        bool result = false;

        yesButton.Click += (s, e) => { result = true; dialog.Close(); };
        cancelButton.Click += (s, e) => { result = false; dialog.Close(); };

        buttonPanel.Children.Add(yesButton);
        buttonPanel.Children.Add(cancelButton);

        content.Children.Add(buttonPanel);
        dialog.Content = content;

        await dialog.ShowDialog(owner);
        return result;
    }
}