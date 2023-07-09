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

        public async Task<IList<ImageModel>> GetAllDataFromFilesAsync()
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

            var requestedData = await response.Content.ReadAsAsync<IList<ImageDto>>();
            var fileModelList = requestedData.Select(imageDto => new ImageModel
            {
                Id = imageDto.Id, Name = imageDto.Name, DateTime = imageDto.DateTime.LocalDateTime,
                CreationTime = imageDto.CreationTime.LocalDateTime, FileSize = imageDto.FileSize
            }).ToList();

            return fileModelList;
        }

        public async Task<ImageDto> GetFileAsync(long id)
        {
            var response = await CreateHttpClient().GetAsync($"api/FileUpload/GetById?id={id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error getting the file!");
            }

            return await response.Content.ReadAsAsync<ImageDto>().ConfigureAwait(false);
        }

        public async Task AddFileAsync(ImageDto fileModel)
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

        public async Task UpdateAsync(ImageDto fileModel)
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