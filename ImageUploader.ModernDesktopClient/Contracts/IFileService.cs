using System.Windows.Media;

namespace ImageUploader.ModernDesktopClient.Contracts;

public interface IFileService
{
    public byte[]? ImageByteArray { get; set; }
    ImageSource OpenFileAndGetImageSource();
}