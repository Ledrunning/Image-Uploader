using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Uploader.Model;

namespace Uploader
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string url = "http://localhost:59871";
        private byte[] _image;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
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
                _image = File.ReadAllBytes(openFileDialog.FileName);
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
                Id = new Guid(),
                Name = $"MyPhoto_{DateTime.UtcNow.ToString("MMddyyyy_HHmmss")}",
                DateTime = DateTimeOffset.Now,
                Photo = Convert.ToBase64String(_image)
            };

            var client = new FileSender(url);
            client.AddFile(fileModel);

            MessageBox.Show("File has been uploaded");
        }


        private async void OnGetFilesClick(object sender, RoutedEventArgs e)
        {
            var client = new FileSender(url);

            try
            {
                Guid.TryParseExact(txtId.Text, "D", out var result);
                var files = await client.GetFileAsync(result);
                var buffer = Convert.FromBase64String(files.Photo);
                imgPhoto.Source = ByteToImage(buffer);
            }
            catch (NullReferenceException err)
            {
                MessageBox.Show(err.Message);
            }
        }


        public ImageSource ByteToImage(byte[] byteArrayIn)
        {
            var bitmapImage = new BitmapImage();
            using (var memoryStream = new MemoryStream(byteArrayIn))
            {
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}