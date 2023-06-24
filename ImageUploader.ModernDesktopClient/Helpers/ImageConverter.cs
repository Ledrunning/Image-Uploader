using System.IO;
using System.Windows.Media.Imaging;

namespace ImageUploader.ModernDesktopClient.Helpers;

public class ImageConverter
{
    public static BitmapImage ByteToImage(byte[]? imageData)
    {
        var image = new BitmapImage();
        if (imageData != null)
        {
            using var memoryStream = new MemoryStream(imageData);
            memoryStream.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = memoryStream;
            image.EndInit();
        }

        image.Freeze();
        return image;
    }
}