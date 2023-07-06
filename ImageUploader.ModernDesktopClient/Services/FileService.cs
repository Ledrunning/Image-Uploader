using System;
using System.IO;
using System.Windows.Interop;
using System.Windows.Media;
using ImageUploader.ModernDesktopClient.Contracts;
using ImageUploader.ModernDesktopClient.Helpers;
using Microsoft.Win32;

namespace ImageUploader.ModernDesktopClient.Services;

public class FileService : IFileService
{
    private const string Filter = "JPEG(*.jpg)|*.jpg|All(*.*)|*";
    private const double ByteToMegabyteCoefficient = 0.000001;
    private readonly OpenFileDialog _openFileDialog;

    public FileService(OpenFileDialog openFileDialog)
    {
        _openFileDialog = openFileDialog;
        _openFileDialog.Filter = Filter;
    }

    public byte[]? ImageByteArray { get; set; }

    public ImageSource OpenFileAndGetImageSource()
    {
        if (!_openFileDialog.ShowDialog().HasValue)
        {
            return new D3DImage();
        }

        ImageByteArray = File.ReadAllBytes(_openFileDialog.FileName);

        return ImageConverter.ByteToImage(ImageByteArray);
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
}