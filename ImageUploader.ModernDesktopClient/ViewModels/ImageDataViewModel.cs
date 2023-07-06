﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageUploader.DesktopCommon.Contracts;
using ImageUploader.DesktopCommon.Events;
using ImageUploader.DesktopCommon.Models;
using ImageUploader.ModernDesktopClient.Contracts;
using ImageUploader.ModernDesktopClient.Helpers;

namespace ImageUploader.ModernDesktopClient.ViewModels;

public partial class ImageDataViewModel : BaseViewModel
{
    private readonly IFileRestService _fileRestService;
    private readonly IFileService _fileService;

    [ObservableProperty] private string? _fileName;
    [ObservableProperty] private bool _isDataLoadIndeterminate;
    [ObservableProperty] private Visibility _isDataLoadVisible = Visibility.Hidden;
    private bool _isImageChanged;

    private bool _isInitialized;

    [ObservableProperty] private List<FileModel> _loadedData = new();

    [ObservableProperty] private Image _loadedImage = new();

    [ObservableProperty] private ObservableCollection<FileModel> _rowCollection = new();

    [ObservableProperty] private FileModel? _selectedItem;

    public ImageDataViewModel(IFileRestService fileRestService,
        IMessageBoxService messageBoxService,
        IFileService fileService,
        DashboardViewModel dashboardViewModel) : base(messageBoxService)
    {
        _fileRestService = fileRestService;
        _fileService = fileService;
        dashboardViewModel.FileEvent += OnFileEvent;
    }

    public override void OnNavigatedTo()
    {
        if (!_isInitialized)
        {
            UpdateDataGrid();
        }
    }

    private void OnFileEvent(TemplateEventArgs<bool>? eventArgs)
    {
        if (eventArgs is { GenericObject: true })
        {
            UpdateDataGrid();
        }
    }

    private async void UpdateDataGrid()
    {
        IsDataLoadVisible = Visibility.Visible;
        IsDataLoadIndeterminate = true;
        var receivedData = await _fileRestService.GetAllDataFromFilesAsync();

        foreach (var fileModel in receivedData)
        {
            RowCollection.Add(fileModel);
        }

        _isInitialized = true;

        IsDataLoadVisible = Visibility.Hidden;
        IsDataLoadIndeterminate = false;
    }

    public async Task DownloadImage()
    {
        try
        {
            if (SelectedItem != null)
            {
                await ExecuteTask(async id =>
                {
                    var files = await _fileRestService.GetFileAsync(id);
                    LoadedImage.Source = ImageConverter.ByteToImage(files.Photo);
                    FileName = SelectedItem.Name;
                }, SelectedItem.Id);
            }
        }
        catch (Exception)
        {
            MessageBoxService.ModernMessageBox.Show("Title", "Could not load image data!");
        }
    }

    //BUG an error occur when the open dialog is close 
    [RelayCommand]
    protected override void OnFileOpen()
    {
        try
        {
            LoadedImage.Source = _fileService.OpenFileAndGetImageSource();
            MessageBoxService.ModernMessageBox.Show("Information!", "File has been opened");
            _isImageChanged = true;
        }
        catch (IOException)
        {
            MessageBoxService.ModernMessageBox.Show("Error!", "Could not open the file!");
        }
    }

    [RelayCommand]
    public async Task DeleteFile()
    {
        if (SelectedItem == null)
        {
            MessageBoxService.ModernMessageBox.Show("Attention!", "Selected row has incorrect or no data.");
            return;
        }

        if (SelectedItem.Id is 0 or < 0)
        {
            MessageBoxService.ModernMessageBox.Show("Attention!", "Selected Id is incorrect.");
            return;
        }

        try
        {
            await ExecuteTask(async id => { await _fileRestService.DeleteAsync(id); }, SelectedItem.Id);

            RowCollection.Clear();
            UpdateDataGrid();
        }
        catch (Exception)
        {
            MessageBoxService.ModernMessageBox.Show("Error!", "Could not delete the file");
        }
    }

    //TODO need to sorted out problem without opening dialog window
    [RelayCommand]
    public async Task UpdateFile()
    {
        try
        {
            var fileInfo = _fileService.GetFileData(_fileService.GetFilepath());

            var fileDto = new FileDto
            {
                Id = SelectedItem!.Id,
                Name = FileName,
                LastPhotoName = SelectedItem?.Name,
                DateTime = DateTimeOffset.UtcNow,
                CreationTime = fileInfo.creationData,
                FileSize = fileInfo.fileSize,
                Photo = _fileService.ImageByteArray,
                IsUpdated = _isImageChanged
            };

            UpdateDataGrid();

            await ExecuteTask(async model => await _fileRestService.UpdateAsync(model), fileDto);
        }
        catch (Exception)
        {
            MessageBoxService.ModernMessageBox.Show("Error!", "Can not update the file");
        }
    }
}