using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    private readonly IFileService _fileService;
    private readonly MessageBox _messageBox;

    [ObservableProperty] private string? _fileName;
    private bool _isImageChanged;
    private bool _isInitialized;

    [ObservableProperty] private List<FileModel> _loadedData = new();

    [ObservableProperty] private Image _loadedImage = new();

    [ObservableProperty] private ObservableCollection<FileModel> _rowCollection = new();

    [ObservableProperty] private FileModel? _selectedItem;

    public ImageDataViewModel(IFileRestService fileRestService,
        IMessageBoxService messageBoxService,
        IFileService fileService)
    {
        _fileRestService = fileRestService;
        _fileService = fileService;
        _messageBox = messageBoxService.InitializeMessageBox();
    }

    public void OnNavigatedTo()
    {
        if (!_isInitialized)
        {
            InitializeDataGrid();
        }
    }

    public void OnNavigatedFrom()
    {
    }

    private void InitializeDataGrid()
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
        try
        {
            if (SelectedItem != null)
            {
                var downloadedFile = await _fileRestService.GetFileAsync(SelectedItem.Id);
                LoadedImage.Source = ImageConverter.ByteToImage(downloadedFile.Photo);
                FileName = SelectedItem.Name;
            }
            else
            {
                _messageBox.Show("Error!", "SelectedItem is null.");
            }
        }
        catch (Exception)
        {
            _messageBox.Show("Title", "Could not load image data!");
        }
    }

    //TODO an error occur when the open dialog is close 
    [RelayCommand]
    private void OnFileOpen()
    {
        try
        {
            LoadedImage.Source = _fileService.OpenFileAndGetImageSource();
            _messageBox.Show("Information!", "File has been opened");
            _isImageChanged = true;
        }
        catch (IOException)
        {
            _messageBox.Show("Error!", "Could not open the file!");
        }
    }

    [RelayCommand]
    public async Task DeleteFile()
    {
        try
        {
            if (SelectedItem != null)
            {
                await _fileRestService.DeleteAsync(SelectedItem.Id);
                RowCollection.Clear();
                InitializeDataGrid();
            }
            else
            {
                _messageBox.Show("Error!", "SelectedItem is null.");
            }
        }
        catch (Exception)
        {
            _messageBox.Show("Error!", "Could not delete the file");
        }
    }

    [RelayCommand]
    public async Task UpdateFile()
    {
        try
        {
            var fileDto = new FileDto
            {
                Id = SelectedItem!.Id,
                Name = FileName,
                LastPhotoName = SelectedItem?.Name,
                DateTime = DateTimeOffset.UtcNow,
                Photo = _fileService.ImageByteArray,
                IsUpdated = _isImageChanged
            };
            await _fileRestService.UpdateAsync(fileDto);
        }
        catch (Exception)
        {
            _messageBox.Show("Error!", "Can not update the file");
        }
    }
}