using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Uploader.Model;

namespace Uploader
{
    public class FileSender
    {
        private string baseAddress;

        public FileSender(string baseAddress)
        {
            this.baseAddress = baseAddress;
        }

        public async Task<FileModel> GetFileAsync(Guid id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;
                response = await client.GetAsync("api/FileUpload/" + id);
                if (response.IsSuccessStatusCode)
                {
                    FileModel user = await response.Content.ReadAsAsync<FileModel>();
                    return user;
                }
            }
            return null;
        }

        public async Task<FileModel> GetAllAsync(int id)
        {
            //using (HttpClient client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(baseAddress);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response;
            //    response = await client.GetAsync("api/FileUpload/" + id);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        FileModel user = await response.Content.ReadAsAsync<FileModel>();
            //        return user;
            //    }
            //}
            return null;
        }

        public async void AddFile(FileModel fileModel)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response;

                response = await client.PostAsJsonAsync<FileModel>("api/FileUpload", fileModel);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Error when adding file!");
                }
            }
        }
    }
}
