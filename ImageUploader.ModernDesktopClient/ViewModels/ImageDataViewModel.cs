using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageUploader.DesktopCommon.Contracts;
using ImageUploader.DesktopCommon.Models;
using ImageUploader.ModernDesktopClient.Contracts;
using ImageUploader.ModernDesktopClient.Helpers;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls;

namespace ImageUploader.ModernDesktopClient.ViewModels;

public partial class ImageDataViewModel : ObservableObject, INavigationAware
{
    private readonly IFileRestService _fileRestService;
    private bool _isInitialized;
    private readonly MessageBox _messageBox;

    [ObservableProperty] private List<FileModel> _loadedData = new();

    [ObservableProperty] private Image _loadedImage = new();

    [ObservableProperty] private ObservableCollection<FileModel> _rowCollection = new();

    [ObservableProperty] private FileModel? _selectedItem;

    public ImageDataViewModel(IFileRestService fileRestService, IMessageBoxService messageBoxService)
    {
        _fileRestService = fileRestService;
        _messageBox = messageBoxService.InitializeMessageBox();
    }

    public void OnNavigatedTo()
    {
        if (!_isInitialized)
        {
            InitializeViewModel();
        }
    }

    public void OnNavigatedFrom()
    {
    }

    private void InitializeViewModel()
    {
        var receivedData = _fileRestService.GetAllDataFromFilesAsync().Result.ToList();

        foreach (var fileModel in receivedData)
        {
            RowCollection.Add(fileModel);
        }

        _isInitialized = true;
    }

    public async Task DownloadImage()
    {
        if (SelectedItem != null)
        {
            var downloadedFile = await _fileRestService.GetFileAsync(SelectedItem.Id);
            LoadedImage.Source = ImageConverter.ByteToImage(downloadedFile.Photo);
        }
        else
        {

        }
    }
}