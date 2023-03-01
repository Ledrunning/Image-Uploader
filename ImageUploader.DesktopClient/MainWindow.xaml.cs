using System;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using ImageUploader.DesktopClient.Model;
using Microsoft.Win32;

namespace ImageUploader.DesktopClient
{
    public partial class MainWindow : Window
    {
        private readonly string _urlAddress = ConfigurationManager.AppSettings["serverUriString"];
        private byte[] _imageByteArray;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnOpenFileClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "JPEG(*.jpg)|*.jpg|All(*.*)|*"
            };

            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            try
            {
                _imageByteArray = File.ReadAllBytes(openFileDialog.FileName);

                imgPhoto.Source = ByteToImage(_imageByteArray);

                MessageBox.Show("File has been opened");
            }
            catch (IOException err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void OnUploadClick(object sender, RoutedEventArgs e)
        {
            var fileModel = new FileModel
            {
                Name = $"MyPhoto_{DateTime.UtcNow:MMddyyyy_HHmmss}.jpg",
                DateTime = DateTimeOffset.Now,
                Photo = _imageByteArray
            };

            var client = new FileSender(_urlAddress);
            client.AddFile(fileModel);

            MessageBox.Show("File has been uploaded");
        }


        private async void OnGetFilesClick(object sender, RoutedEventArgs e)
        {
            var client = new FileSender(_urlAddress);

            try
            {
                int.TryParse(txtId.Text, out var result);
                var files = await client.GetFileAsync(result);
                imgPhoto.Source = ByteToImage(files.Photo);
            }
            catch (NullReferenceException err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private static BitmapImage ByteToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            var image = new BitmapImage();
            using (var memoryStream = new MemoryStream(imageData))
            {
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
}