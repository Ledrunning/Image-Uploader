using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageUploader.DesktopCommon.Contracts;
using ImageUploader.DesktopCommon.Models;
using Microsoft.Win32;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace ImageUploader.ModernDesktopClient.ViewModels;

public partial class DashboardViewModel : ObservableObject, INavigationAware
{
    private readonly IFileRestService _fileRestService;

    public DashboardViewModel(IFileRestService fileRestService)
    {
        _fileRestService = fileRestService;
    }

    private readonly MessageBox _messageBox = new()
    {
        ButtonLeftName = "Ok",
        ButtonRightName = "Cancel"
    };

    [ObservableProperty] private int _counter;

    [ObservableProperty] private Image _loadedImage = new ();
    
    private byte[]? ImageByteArray { get; set; }

    public void OnNavigatedTo()
    {
    }

    public void OnNavigatedFrom()
    {
    }

    [RelayCommand]
    private void OnCounterIncrement()
    {
        Counter++;
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

            LoadedImage.Source = ByteToImage(ImageByteArray);
            _messageBox.Show("Information!", "File has been opened");
        }
        catch (IOException e)
        {
            _messageBox.Show("Error!", e.Message);
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
        var fileModel = new FileModel
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

    private static BitmapImage ByteToImage(byte[]? imageData)
    {
        var image = new BitmapImage();
        if (imageData != null)
        {
            using var memoryStream = new MemoryStream(imageData);
            memoryStream.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = memoryStream;
            image.EndInit();
        }

        image.Freeze();
        return image;
    }
}