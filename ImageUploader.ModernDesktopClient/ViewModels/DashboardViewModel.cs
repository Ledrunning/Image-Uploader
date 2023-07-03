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
using ImageUploader.ModernDesktopClient.Enums;
using ImageUploader.ModernDesktopClient.Helpers;

namespace ImageUploader.ModernDesktopClient.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly IFileRestService _fileRestService;
    private readonly IFileService _fileService;
    private readonly IMessageBoxService _messageBoxService;
    private ButtonName _buttonName;

    [ObservableProperty] private long _fileId;
    [ObservableProperty] private Image _loadedImage = new();

    public DashboardViewModel(IFileRestService fileRestService,
        IMessageBoxService messageBoxService,
        IFileService fileService)
    {
        _fileRestService = fileRestService;
        _messageBoxService = messageBoxService;
        _fileService = fileService;
        _messageBoxService.ButtonEvent += OnOkButtonClick;
    }
    
    private void OnOkButtonClick(TemplateEventArgs<ButtonName> e)
    {
        _buttonName = e.GenericObject;
    }

    public event TemplateEventHandler<bool>? FileEvent = delegate { };

    //TODO an error occur when the open dialog is close 
    [RelayCommand]
    private void OnFileOpen()
    {
        try
        {
            LoadedImage.Source = _fileService.OpenFileAndGetImageSource();
            _messageBoxService.ModernMessageBox.Show("Information!", "File has been opened");
        }
        catch (IOException)
        {
            _messageBoxService.ModernMessageBox.Show("Error!", "Could not load the file!");
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
                _messageBoxService.ModernMessageBox.Show("Attention!", "Upload file first please!");
            }

            var fileModel = new FileDto
            {
                Name = $"MyPhoto_{DateTime.UtcNow:MMddyyyy_HHmmss}.jpg",
                DateTime = DateTimeOffset.Now,
                Photo = _fileService.ImageByteArray
            };

            await ExecuteTask(async fileDto => { await _fileRestService.AddFileAsync(fileDto); }, fileModel);
            FileEvent?.Invoke(new TemplateEventArgs<bool>(true));

            _messageBoxService.ModernMessageBox.Show("Information!", "File has been uploaded");
        }
        catch (Exception)
        {
            _messageBoxService.ModernMessageBox.Show("Error!", "Could not add the file into server!");
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
            _messageBoxService.ModernMessageBox.Show("Error!", "Could not download the file from server!");
            IsIndeterminate = false;
            IsVisible = Visibility.Hidden;
        }
    }

    [RelayCommand]
    private async Task OnImageDelete()
    {
        if (FileId is 0 or < 0)
        {
            _messageBoxService.ModernMessageBox.Show("Attention!", "Please enter correct Id.");
            return;
        }

        _messageBoxService.ModernMessageBox.Show("Attention!", "Are you sure you want to delete this file?");
        
        if (_buttonName == ButtonName.Ok)
        {
            try
            {
                await ExecuteTask(async id => { await _fileRestService.DeleteAsync(id); }, FileId);
                FileEvent?.Invoke(new TemplateEventArgs<bool>(true));
                _messageBoxService.ModernMessageBox.Show("Information!", "File has been deleted");
                FileId = 0;
            }
            catch (Exception)
            {
                _messageBoxService.ModernMessageBox.Show("Error!", "Could not delete the file from server!");
            }
        }
    }
}