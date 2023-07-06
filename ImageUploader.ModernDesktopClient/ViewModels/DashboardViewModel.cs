using System;
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

public partial class DashboardViewModel : BaseViewModel
{
    private readonly IFileRestService _fileRestService;
    private readonly IFileService _fileService;

    [ObservableProperty] private long _fileId;
    [ObservableProperty] private Image _loadedImage = new();

    public DashboardViewModel(IFileRestService fileRestService, IFileService fileService,
        IMessageBoxService messageBoxService) : base(messageBoxService)
    {
        _fileRestService = fileRestService;
        _fileService = fileService;
    }

    public event TemplateEventHandler<bool>? FileEvent = delegate { };

    //TODO an error occur when the open dialog is close 
    [RelayCommand]
    protected override void OnFileOpen()
    {
        try
        {
            LoadedImage.Source = _fileService.OpenFileAndGetImageSource();
            MessageBoxService.ModernMessageBox.Show("Information!", "File has been opened");
        }
        catch (IOException)
        {
            MessageBoxService.ModernMessageBox.Show("Error!", "Could not load the file!");
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
                MessageBoxService.ModernMessageBox.Show("Attention!", "Upload file first please!");
            }

            var fileInfo = _fileService.GetFileData(_fileService.GetFilepath());

            var fileModel = new FileDto
            {
                Name = $"MyPhoto_{DateTime.UtcNow:MMddyyyy_HHmmss}.jpg",
                DateTime = DateTimeOffset.Now,
                CreationTime = fileInfo.creationData,
                FileSize = fileInfo.fileSize,
                Photo = _fileService.ImageByteArray
            };

            await ExecuteTask(async fileDto => { await _fileRestService.AddFileAsync(fileDto); }, fileModel);

            FileEvent?.Invoke(new TemplateEventArgs<bool>(true));

            MessageBoxService.ModernMessageBox.Show("Information!", "File has been uploaded");
        }
        catch (Exception)
        {
            MessageBoxService.ModernMessageBox.Show("Error!", "Could not add the file into server!");
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
            MessageBoxService.ModernMessageBox.Show("Error!", "Could not download the file from server!");
            IsIndeterminate = false;
            IsVisible = Visibility.Hidden;
        }
    }

    [RelayCommand]
    private async Task OnImageDelete()
    {
        if (FileId is 0 or < 0)
        {
            MessageBoxService.ModernMessageBox.Show("Attention!", "Please enter correct Id.");
            return;
        }

        try
        {
            await ExecuteTask(async id => { await _fileRestService.DeleteAsync(id); }, FileId);
            FileEvent?.Invoke(new TemplateEventArgs<bool>(true));
            MessageBoxService.ModernMessageBox.Show("Information!", "File has been deleted");
            FileId = 0;
        }
        catch (Exception)
        {
            MessageBoxService.ModernMessageBox.Show("Error!", "Could not delete the file from server!");
        }
    }
}