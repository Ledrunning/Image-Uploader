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
using ImageUploader.DesktopCommon.Enum;
using ImageUploader.DesktopCommon.Events;
using ImageUploader.DesktopCommon.Models;
using ImageUploader.ModernDesktopClient.Contracts;
using ImageUploader.ModernDesktopClient.Helpers;
using ImageUploader.ModernDesktopClient.Resources;

namespace ImageUploader.ModernDesktopClient.ViewModels;

public partial class ImageDataViewModel : BaseViewModel
{
    private readonly IFileRestService _fileRestService;
    private readonly IFileService _fileService;

    [ObservableProperty] private string? _fileName;
    private FileUpdate _fileUpdate = FileUpdate.NoOperation;
    [ObservableProperty] private bool _isDataLoadIndeterminate;
    [ObservableProperty] private Visibility _isDataLoadVisible = Visibility.Hidden;

    private bool _isInitialized;

    [ObservableProperty] private List<ImageModel> _loadedData = new();

    [ObservableProperty] private Image _loadedImage = new();

    [ObservableProperty] private ObservableCollection<ImageModel> _rowCollection = new();

    [ObservableProperty] private ImageModel? _selectedItem;

    [ObservableProperty] private string _imageId;

    public ImageDataViewModel(IFileRestService fileRestService,
        IMessageBoxService messageBoxService,
        IFileService fileService,
        DashboardViewModel dashboardViewModel) : base(messageBoxService, fileService)
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

    [RelayCommand]
    public async Task OnDownloadImage()
    {
        try
        {
            if (SelectedItem != null)
            {
                await ExecuteTask(async id =>
                {
                    var imageDto = await _fileRestService.GetFileAsync(id);
                    ImageBuffer = imageDto.Photo;
                    ImageName = imageDto.Name;
                    LoadedImage.Source = ImageConverter.ByteToImage(imageDto.Photo);
                    FileName = SelectedItem.Name;

                    ImageId = imageDto.Id.ToString();
                }, SelectedItem.Id);
            }
        }
        catch (Exception)
        {
            MessageBoxService.ModernMessageBox.Show(BaseMessages.Error, ImageDataMessages.FileLoadErrorMessage);
        }
    }

    //BUG an error occur when the open dialog is close 
    [RelayCommand]
    protected override void OnFileOpen()
    {
        try
        {
            var (imageSource, isNotCancel) = _fileService.OpenFileAndGetImageSource();
            if (!isNotCancel)
            {
                return;
            }

            LoadedImage.Source = imageSource;
            MessageBoxService.ModernMessageBox.Show(BaseMessages.Information, ImageDataMessages.FileOpenedMessage);
            _fileUpdate = FileUpdate.DeleteAndSave;
        }
        catch (IOException)
        {
            MessageBoxService.ModernMessageBox.Show(BaseMessages.Error, ImageDataMessages.FileOpenErrorMessage);
        }
    }

    [RelayCommand]
    public async Task DeleteFile()
    {
        if (SelectedItem == null)
        {
            MessageBoxService.ModernMessageBox.Show(BaseMessages.Attention, ImageDataMessages.SelectedRowErrorMessage);
            return;
        }

        if (SelectedItem.Id is 0 or < 0)
        {
            MessageBoxService.ModernMessageBox.Show(BaseMessages.Attention, ImageDataMessages.SelectedIdError);
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
            MessageBoxService.ModernMessageBox.Show(BaseMessages.Error, ImageDataMessages.DeleteErrorMessage);
        }
    }

    [RelayCommand]
    public void SaveFileToLocalStorage()
    {
        try
        {
            SaveImage();
        }
        catch (Exception)
        {
            MessageBoxService.ModernMessageBox.Show(BaseMessages.Error, "Can not save image to local storage");
        }
    }

    [RelayCommand]
    public async Task UpdateFile()
    {
        try
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.Name != FileName)
                {
                    _fileUpdate = FileUpdate.Rewrite;
                }

                var imageDto = new ImageDto
                {
                    Id = SelectedItem.Id,
                    Name = FileName,
                    LastPhotoName = SelectedItem.Name,
                    DateTime = DateTimeOffset.UtcNow,
                    CreationTime = SelectedItem.CreationTime,
                    FileSize = SelectedItem.FileSize,
                    Photo = _fileService.ImageByteArray,
                    FileUpdate = _fileUpdate
                };

                await ExecuteTask(async model => await _fileRestService.UpdateAsync(model), imageDto);

                UpdateDataGrid();
            }
        }
        catch (Exception)
        {
            MessageBoxService.ModernMessageBox.Show(BaseMessages.Error, "Can not update the file");
        }
        finally
        {
            _fileUpdate = FileUpdate.NoOperation;
        }
    }

    [RelayCommand]
    public void UpdateDataManually()
    {
        UpdateDataGrid();
    }
}