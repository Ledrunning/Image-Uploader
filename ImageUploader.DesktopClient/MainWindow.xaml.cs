using System;
using System.Configuration;
using System.IO;
using System.Windows;

using ImageUploader.DesktopCommon;
using Microsoft.Win32;
using ImageUploader.DesktopCommon.Models;
using ImageUploader.DesktopCommon.Rest;
using System.Windows.Media;

namespace ImageUploader.DesktopClient
{
    public partial class MainWindow : Window
    {
        private readonly FileRestService _client;
        private readonly string _urlAddress = ConfigurationManager.AppSettings["serverUriString"];
        private byte[] _imageByteArray;

        public MainWindow()
        {
            InitializeComponent();
            _client = new FileRestService(_urlAddress);
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

                var converter = new ImageSourceConverter();
                ImgPhoto.Source = (ImageSource)converter.ConvertFrom(FileService.ByteToImage(_imageByteArray));

                MessageBox.Show("File has been opened");
            }
            catch (IOException err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private async void OnUploadClick(object sender, RoutedEventArgs e)
        {
            var fileModel = new FileModel
            {
                Name = $"MyPhoto_{DateTime.UtcNow:MMddyyyy_HHmmss}.jpg",
                DateTime = DateTimeOffset.Now,
                Photo = _imageByteArray
            };

            await _client.AddFileAsync(fileModel);

            MessageBox.Show("File has been uploaded");
        }

        private async void OnDownloadClick(object sender, RoutedEventArgs e)
        {
            try
            {
                long.TryParse(txtId.Text, out var result);

                DownloadProgressBar.IsIndeterminate = true;
                var files = await _client.GetFileAsync(result);
                DownloadProgressBar.IsIndeterminate = false;

                var converter = new ImageSourceConverter();
                ImgPhoto.Source = (ImageSource)converter.ConvertFrom(FileService.ByteToImage(files.Photo));
            }
            catch (NullReferenceException err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private async void OnDeleteImageClick(object sender, RoutedEventArgs e)
        {
            try
            {
                long.TryParse(txtId.Text, out var id);
                await _client.DeleteAsync(id);

                MessageBox.Show("File has been deleted");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void OnClearImageClick(object sender, RoutedEventArgs e)
        {
            ImgPhoto.Source = null;
        }
    }
}