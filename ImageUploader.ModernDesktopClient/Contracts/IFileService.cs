using System;
using System.Windows.Media;

namespace ImageUploader.ModernDesktopClient.Contracts;

public interface IFileService
{
    public byte[]? ImageByteArray { get; set; }
    ImageSource OpenFileAndGetImageSource();
    (DateTime creationData, double fileSize) GetFileData(string filePath);
    string GetFilepath();

    void SaveImage(string fileName, byte[] imageBuffer);
}