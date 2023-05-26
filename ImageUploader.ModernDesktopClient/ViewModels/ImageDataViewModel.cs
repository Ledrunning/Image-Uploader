using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageUploader.DesktopCommon.Contracts;
using ImageUploader.DesktopCommon.Models;
using Wpf.Ui.Common.Interfaces;

namespace ImageUploader.ModernDesktopClient.ViewModels;

public partial class ImageDataViewModel : ObservableObject, INavigationAware
{
    private readonly IFileRestService _fileRestService;
    private bool _isInitialized;
    [ObservableProperty] private List<FileModel> _loadedData = new();

    public ImageDataViewModel(IFileRestService fileRestService)
    {
        _fileRestService = fileRestService;
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
        LoadedData = receivedData;
        _isInitialized = true;
    }
}