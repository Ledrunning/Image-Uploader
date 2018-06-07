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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Uploader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] image;
        private string FileName { get; set; }
            
        private readonly string url = "http://127.0.0.1:18710/";

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
                catch(IOException err)
                {
                    MessageBox.Show(err.Message);
                }
                
            }
              
        }

        private async void Upload_Click(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response = await UploadImage(url, image);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("File has been uploaded");
            }
                
        }

        async public Task<HttpResponseMessage> UploadImage(string url, byte[] ImageData)
        {
            var client = new HttpClient();

            var requestContent = new MultipartFormDataContent();
            
            var imageContent = new ByteArrayContent(ImageData);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            requestContent.Add(imageContent, FileName);

            return await client.PostAsync(url, requestContent);
        }
    }
}
