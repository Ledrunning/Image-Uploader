using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageUploader.DesktopCommon.Events;
using ImageUploader.ModernDesktopClient.Contracts;
using ImageUploader.ModernDesktopClient.Enums;
using Wpf.Ui.Common.Interfaces;

namespace ImageUploader.ModernDesktopClient.ViewModels;

public abstract partial class BaseViewModel : ObservableObject, INavigationAware
{
    protected readonly IMessageBoxService MessageBoxService;
    [ObservableProperty] private bool _isIndeterminate;

    [ObservableProperty] private Visibility _isVisible = Visibility.Hidden;
    protected ButtonName ButtonName;

    protected BaseViewModel(IMessageBoxService messageBoxService)
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

    protected abstract void OnFileOpen();

    protected async Task ExecuteTask<T>(Func<T, Task> function, T data)
    {
        try
        {
            IsVisible = Visibility.Visible;
            IsIndeterminate = true;
            await function(data).ConfigureAwait(false);
            IsIndeterminate = false;
            IsVisible = Visibility.Hidden;
        }
        catch (Exception)
        {
            IsIndeterminate = false;
            IsVisible = Visibility.Hidden;
            throw;
        }
    }
}