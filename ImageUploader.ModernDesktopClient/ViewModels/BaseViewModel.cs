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
    private readonly IFileService _fileService;
    protected readonly IMessageBoxService MessageBoxService;
    [ObservableProperty] private bool _isIndeterminate;

    [ObservableProperty] private Visibility _isVisible = Visibility.Hidden;
    protected ButtonName ButtonName;
    protected byte[]? ImageBuffer;
    protected string? ImageName;

    protected BaseViewModel(IMessageBoxService messageBoxService, IFileService fileService)
    {
        MessageBoxService = messageBoxService;
        _fileService = fileService;
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

    protected void SaveImage()
    {
        if (ImageName == null || ImageBuffer == null || ImageBuffer.Length == 0)
        {
            MessageBoxService.ModernMessageBox.Show("Error!", "Please select the image");
            return;
        }

        _fileService.SaveImage(ImageName, ImageBuffer);
    }
}