using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageUploader.DesktopCommon.Contracts;
using ImageUploader.DesktopCommon.Models;
using ImageUploader.ModernDesktopClient.Helpers;
using Microsoft.Win32;
using Wpf.Ui.Common.Interfaces;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace ImageUploader.ModernDesktopClient.ViewModels;

public partial class DashboardViewModel : ObservableObject, INavigationAware
{
    private readonly IFileRestService _fileRestService;

    private readonly MessageBox _messageBox = new()
    {
        ButtonLeftName = "Ok",
        ButtonRightName = "Cancel"
    };

    [ObservableProperty] private long _fileId;

    [ObservableProperty] private bool _isIndeterminate;

    [ObservableProperty] private Visibility _isVisible = Visibility.Hidden;

    [ObservableProperty] private Image _loadedImage = new();

    public DashboardViewModel(IFileRestService fileRestService)
    {
        _fileRestService = fileRestService;
        _messageBox.ButtonLeftClick += OnMessageBoxButtonLeftClick;
        _messageBox.ButtonRightClick += OnMessageBoxButtonRightClick;
    }

    private byte[]? ImageByteArray { get; set; }

    public void OnNavigatedTo()
    {
    }

    public void OnNavigatedFrom()
    {
    }

    private void OnMessageBoxButtonRightClick(object sender, RoutedEventArgs e)
    {
        _messageBox.Visibility = Visibility.Hidden;
    }

    private void OnMessageBoxButtonLeftClick(object sender, RoutedEventArgs e)
    {
        _messageBox.Visibility = Visibility.Hidden;
    }

    [RelayCommand]
    private void OnFileOpen()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "JPEG(*.jpg)|*.jpg|All(*.*)|*"
        };

        if (openFileDialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            ImageByteArray = File.ReadAllBytes(openFileDialog.FileName);

            LoadedImage.Source = ImageConverter.ByteToImage(ImageByteArray);
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
            var fileModel = new FileDto
            {
                Name = $"MyPhoto_{DateTime.UtcNow:MMddyyyy_HHmmss}.jpg",
                DateTime = DateTimeOffset.Now,
                Photo = ImageByteArray
            };

            if (ImageByteArray is { Length: 0 })
            {
                _messageBox.Show("Attention!", "Upload file first please!");
            }

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
            await ExecuteTask(async id =>
            {
                await _fileRestService.DeleteAsync(id);
            }, FileId);

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