using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using ImageUploader.ModernDesktopClient.Contracts;
using Microsoft.Win32;
using ImageConverter = ImageUploader.ModernDesktopClient.Helpers.ImageConverter;

namespace ImageUploader.ModernDesktopClient.Services;

public class FileService : IFileService
{
    private const string Filter = "JPEG(*.jpg)|*.jpg|All(*.*)|*";
    private const string SaveFilter = "JPEG(*.jpg)|*.jpg|All(*.*)|*";
    private const double ByteToMegabyteCoefficient = 0.000001;
    private readonly OpenFileDialog _openFileDialog;
    private readonly SaveFileDialog _saveFileDialog;

    public FileService(OpenFileDialog openFileDialog, SaveFileDialog saveFileDialog)
    {
        _openFileDialog = openFileDialog;
        _saveFileDialog = saveFileDialog;
        _openFileDialog.Filter = Filter;
        _saveFileDialog.Filter = SaveFilter;
    }

    public byte[]? ImageByteArray { get; set; }

    //BUG! If open and cancel
    public (ImageSource imageSource, bool isNotCancel) OpenFileAndGetImageSource()
    {
        if (!_openFileDialog.ShowDialog().HasValue)
        {
            return (new D3DImage(), isNotCancel: false);
        }

        if (string.IsNullOrWhiteSpace(_openFileDialog.FileName))
        {
            return (new D3DImage(), isNotCancel: false);
        }

        ImageByteArray = File.ReadAllBytes(_openFileDialog.FileName);

        _openFileDialog.FileName = string.Empty;
        return (ImageConverter.ByteToImage(ImageByteArray), isNotCancel: true);
    }

    public (DateTime creationData, double fileSize) GetFileData(string filePath)
    {
        var creationTime = File.GetCreationTime(filePath);
        var fileLength = new FileInfo(filePath).Length * ByteToMegabyteCoefficient;

        return (creationTime, fileLength);
    }

    public string GetFilepath()
    {
        return _openFileDialog.FileName;
    }

    public void SaveImage(string fileName, byte[] imageBuffer)
    {
        _saveFileDialog.FileName = fileName;
        if (!_saveFileDialog.ShowDialog().HasValue)
        {
            return;
        }

        using var memoryStream = new MemoryStream(imageBuffer);
        var image = Image.FromStream(memoryStream);
        image.Save(_saveFileDialog.FileName, ImageFormat.Jpeg);
    }
}