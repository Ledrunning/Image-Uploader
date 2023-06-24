using System;
using System.IO;
using System.Windows.Media;
using ImageUploader.ModernDesktopClient.Contracts;
using ImageUploader.ModernDesktopClient.Helpers;
using Microsoft.Win32;

namespace ImageUploader.ModernDesktopClient.Services;

public class FileService : IFileService
{
    private const string Filter = "JPEG(*.jpg)|*.jpg|All(*.*)|*";
    private readonly OpenFileDialog _openFileDialog;

    public FileService(OpenFileDialog openFileDialog)
    {
        _openFileDialog = openFileDialog;
        _openFileDialog.Filter = Filter;
    }

    public byte[]? ImageByteArray { get; set; }

    public ImageSource OpenFileAndGetImageSource()
    {
        _openFileDialog.ShowDialog();

        ImageByteArray = File.ReadAllBytes(_openFileDialog.FileName);

        return ImageConverter.ByteToImage(ImageByteArray);

    }
}