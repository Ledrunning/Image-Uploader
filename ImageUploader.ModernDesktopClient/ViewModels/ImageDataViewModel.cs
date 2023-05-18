using System.Collections;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using ImageUploader.DesktopCommon.Contracts;
using Wpf.Ui.Common.Interfaces;

namespace ImageUploader.ModernDesktopClient.ViewModels;

public partial class ImageDataViewModel : ObservableObject, INavigationAware
{
    private readonly IFileRestService _fileRestService;
    [ObservableProperty] private List<string> _loadedData = new();

    public ImageDataViewModel(IFileRestService fileRestService)
    {
        _fileRestService = fileRestService;
        LoadedData = _fileRestService.GetFileAsync(1);
    }

    public void OnNavigatedTo()
    {
        throw new System.NotImplementedException();
    }

    public void OnNavigatedFrom()
    {
        throw new System.NotImplementedException();
    }
}