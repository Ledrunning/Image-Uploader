using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ImageUploader.DesktopCommon.Contracts;
using ImageUploader.DesktopCommon.Models;

namespace ImageUploader.DesktopCommon.Rest
{
    public class FileRestService : IFileRestService
    {
        private readonly string _baseAddress;

        public FileRestService(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public async Task<IList<FileModel>> GetAllDataFromFilesAsync()
        {
            if (string.IsNullOrEmpty(_baseAddress))
            {
                throw new Exception("Empty or Incorrect base URL address!");
            }
            var response = await CreateHttpClient().GetAsync("api/FileUpload/GetAll").ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error getting the files!");
            }

            var requestedData = await response.Content.ReadAsAsync<IList<FileDto>>();
            var fileModelList = requestedData.Select(fileDto => new FileModel { Id = fileDto.Id, Name = fileDto.Name, DateTime = fileDto.DateTime.LocalDateTime }).ToList();

            return fileModelList;
        }

        public async Task<FileDto> GetFileAsync(long id)
        {
            var response = await CreateHttpClient().GetAsync($"api/FileUpload/GetById?id={id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error getting the file!");
            }

            return await response.Content.ReadAsAsync<FileDto>().ConfigureAwait(false);
        }

        public async Task AddFileAsync(FileDto fileModel)
        {
            var response = await CreateHttpClient().PostAsJsonAsync("api/FileUpload/Create", fileModel)
                .ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error when adding file!");
            }
        }

        public async Task DeleteAsync(long id)
        {
            var response = await CreateHttpClient().DeleteAsync($"api/FileUpload/Delete?id={id}").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error when deleting file!");
            }
        }

        public async Task UpdateAsync(FileDto fileModel)
        {
            var response = await CreateHttpClient().PostAsJsonAsync("api/FileUpload/Update", fileModel)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error when updating file!");
            }
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_baseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}