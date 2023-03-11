using System.Drawing;
using System.IO;

namespace ImageUploader.DesktopCommon
{
    public class FileService
    {
        public static Bitmap ByteToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            Bitmap image;
            using (var memoryStream = new MemoryStream(imageData))
            {
                image = new Bitmap(memoryStream);
            }

            return image;
        }
    }
}