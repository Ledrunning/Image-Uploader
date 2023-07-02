using System.IO;
using System.Windows.Interop;
using System.Windows.Media;
using ImageUploader.ModernDesktopClient.Contracts;
using ImageUploader.ModernDesktopClient.Enums;
using ImageUploader.ModernDesktopClient.Helpers;
using Microsoft.Win32;

namespace ImageUploader.ModernDesktopClient.Services;

public class FileService : IFileService
{
    private const string Filter = "JPEG(*.jpg)|*.jpg|All(*.*)|*";
    private readonly IMessageBoxService _mesaBoxService;
    private readonly OpenFileDialog _openFileDialog;

    public FileService(OpenFileDialog openFileDialog, IMessageBoxService mesaBoxService)
    {
        _openFileDialog = openFileDialog;
        _mesaBoxService = mesaBoxService;
        _openFileDialog.Filter = Filter;
        _mesaBoxService.OkButtonEvent += OnOkButtonEvent;
    }

    private void OnOkButtonEvent(DesktopCommon.Events.TemplateEventArgs<ButtonName> templateEventArgs)
    {
        throw new System.NotImplementedException();
    }

    public byte[]? ImageByteArray { get; set; }

    public ImageSource OpenFileAndGetImageSource()
    {
        _openFileDialog.ShowDialog();

        if (_mesaBoxService.ModernMessageBox.ButtonLeftName == ButtonName.Ok.ToString())
        {
            ImageByteArray = File.ReadAllBytes(_openFileDialog.FileName);

            return ImageConverter.ByteToImage(ImageByteArray);
        }

        return new D3DImage();
    }
}