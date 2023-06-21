using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageUploader.DesktopCommon.Contracts;
using ImageUploader.DesktopCommon.Models;
using ImageUploader.ModernDesktopClient.Contracts;
using ImageUploader.ModernDesktopClient.Helpers;
using Wpf.Ui.Common.Interfaces;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace ImageUploader.ModernDesktopClient.ViewModels;

public partial class DashboardViewModel : ObservableObject, INavigationAware
{
    private readonly IFileRestService _fileRestService;
    private readonly IFileService _fileService;
    private readonly MessageBox _messageBox;

    [ObservableProperty] private long _fileId;

    [ObservableProperty] private bool _isIndeterminate;

    [ObservableProperty] private Visibility _isVisible = Visibility.Hidden;

    [ObservableProperty] private Image _loadedImage = new();

    public DashboardViewModel(IFileRestService fileRestService,
        IMessageBoxService messageBoxService,
        IFileService fileService)
    {
        _fileRestService = fileRestService;
        _fileService = fileService;
        _messageBox = messageBoxService.InitializeMessageBox();
    }

    public void OnNavigatedTo()
    {
    }

    public void OnNavigatedFrom()
    {
    }

    //TODO an error occur when the open dialog is close 
    [RelayCommand]
    private void OnFileOpen()
    {
        try
        {
            LoadedImage.Source = _fileService.OpenFileAndGetImageSource();
            _messageBox.Show("Information!", "File has been opened");
        }
        catch (IOException)
        {
            _messageBox.Show("Error!", "Could not load the file!");
        }
    }

    [RelayCommand]
    private void OnImageClear()
    {
        LoadedImage.Source = null;
    }

    [RelayCommand]
    private async Task OnImageUpload()
    {
        try
        {
            if (_fileService.ImageByteArray is { Length: 0 })
            {
                _messageBox.Show("Attention!", "Upload file first please!");
            }

            var fileModel = new FileDto
            {
                Name = $"MyPhoto_{DateTime.UtcNow:MMddyyyy_HHmmss}.jpg",
                DateTime = DateTimeOffset.Now,
                Photo = _fileService.ImageByteArray
            };

            await _fileRestService.AddFileAsync(fileModel);

            _messageBox.Show("Information!", "File has been uploaded");
        }
        catch (Exception)
        {
            _messageBox.Show("Error!", "Could not add the file into server!");
        }
    }

    [RelayCommand]
    private async Task OnImageDownload()
    {
        try
        {
            await ExecuteTask(async id =>
            {
                var files = await _fileRestService.GetFileAsync(id);
                LoadedImage.Source = ImageConverter.ByteToImage(files.Photo);
            }, FileId);
        }
        catch (Exception)
        {
            _messageBox.Show("Error!", "Could not download the file from server!");
            IsIndeterminate = false;
            IsVisible = Visibility.Hidden;
        }
    }

    [RelayCommand]
    private async Task OnImageDelete()
    {
        try
        {
            await ExecuteTask(async id => { await _fileRestService.DeleteAsync(id); }, FileId);

            _messageBox.Show("Information!", "File has been deleted");
        }
        catch (Exception)
        {
            _messageBox.Show("Error!", "Could not delete the file from server!");
        }
    }

    private async Task ExecuteTask<T>(Func<T, Task> function, T data)
    {
        IsVisible = Visibility.Visible;
        IsIndeterminate = true;
        await function(data);
        IsIndeterminate = false;
        IsVisible = Visibility.Hidden;
    }
}