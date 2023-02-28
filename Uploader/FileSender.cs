using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Uploader.Model;

namespace Uploader
{
    public class FileSender
    {
        private readonly string _baseAddress;

        public FileSender(string baseAddress)
        {
            this._baseAddress = baseAddress;
        }

        public async Task<FileModel> GetFileAsync(long id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/FileUpload/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadAsAsync<FileModel>();
                    return user;
                }
            }

            throw new Exception("Error getting the file!");
        }
        
        public async void AddFile(FileModel fileModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsJsonAsync("api/FileUpload", fileModel);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Error when adding file!");
                }
            }
        }
    }
}