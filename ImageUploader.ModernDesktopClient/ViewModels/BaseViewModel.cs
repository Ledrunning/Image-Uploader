using System.Threading.Tasks;
using System.Windows;
using System;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Common.Interfaces;

namespace ImageUploader.ModernDesktopClient.ViewModels;

public partial class BaseViewModel : ObservableObject, INavigationAware
{
    [ObservableProperty] private bool _isIndeterminate;

    [ObservableProperty] private Visibility _isVisible = Visibility.Hidden;
    
    public virtual void OnNavigatedFrom()
    {
    }

    public virtual void OnNavigatedTo()
    {
    }

    protected async Task ExecuteTask<T>(Func<T, Task> function, T data)
    {
        IsVisible = Visibility.Visible;
        IsIndeterminate = true;
        await function(data).ConfigureAwait(false);
        IsIndeterminate = false;
        IsVisible = Visibility.Hidden;
    }
}