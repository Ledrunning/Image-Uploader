using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Uploader.Model;

namespace Uploader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] image;
        private readonly string url = "http://localhost:59871";
        private string FileName { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "JPEG(*.jpg)|*.jpg|All(*.*)|*";

            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.SafeFileName;
                try
                {
                    image = File.ReadAllBytes(openFileDialog.FileName);
                    MessageBox.Show("File has been opened");
                }
                catch (IOException err)
                {
                    MessageBox.Show(err.Message);
                }

            }

        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            Random randomName = new Random();
            FileModel fileModel = new FileModel
            {
                Id = new Guid(),
                Name = "MyPhoto" + randomName.Next().ToString(),
                DateTime = DateTimeOffset.Now,
                Photo = Convert.ToBase64String(image)
            };

            FileSender client = new FileSender(url);
            client.AddFile(fileModel);

            MessageBox.Show("File has been uploaded");
        }



        private async void Get_Click(object sender, RoutedEventArgs e)
        {
            FileSender client = new FileSender(url);
            string[] formats = { "N", "D", "B", "P", "X" };
            Guid result;

            try
            {
                Guid.TryParseExact(txtId.Text, "D", out result);
                var files = await client.GetFileAsync(result);
                var buffer = Convert.FromBase64String(files.Photo);
                imgPhoto.Source = ByteToImage(buffer);
            }
            catch(NullReferenceException err)
            {
                MessageBox.Show(err.Message);
            }
            

        }



        public ImageSource ByteToImage(byte[] byteArrayIn)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(byteArrayIn);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }
    }
}