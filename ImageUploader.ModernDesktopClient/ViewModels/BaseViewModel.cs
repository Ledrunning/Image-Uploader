using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageUploader.DesktopCommon.Events;
using ImageUploader.ModernDesktopClient.Contracts;
using ImageUploader.ModernDesktopClient.Enums;
using Wpf.Ui.Common.Interfaces;

namespace ImageUploader.ModernDesktopClient.ViewModels;

public partial class BaseViewModel : ObservableObject, INavigationAware
{
    protected readonly IMessageBoxService MessageBoxService;
    [ObservableProperty] private bool _isIndeterminate;

    [ObservableProperty] private Visibility _isVisible = Visibility.Hidden;
    protected ButtonName ButtonName;

    public BaseViewModel(IMessageBoxService messageBoxService)
    {
        MessageBoxService = messageBoxService;
        MessageBoxService.ButtonEvent += OnButtonEvent;
    }

    public virtual void OnNavigatedFrom()
    {
    }

    public virtual void OnNavigatedTo()
    {
    }

    private void OnButtonEvent(TemplateEventArgs<ButtonName> eventArgs)
    {
        ButtonName = eventArgs.GenericObject;
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