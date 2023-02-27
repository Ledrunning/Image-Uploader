using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FileUploadWebApiTest.Contracts;
using FileUploadWebApiTest.Models;
using FileUploadWebApiTest.Repository.Entity;

namespace FileUploadWebApiTest.Services
{
    public class FileService : IFileService
    {
        private const string FolderName = "EmployeePhoto";

        private static readonly string PhotoDataPath = Path.GetDirectoryName(
                                                           Assembly.GetExecutingAssembly().Location) +
                                                       $"\\{FolderName}";

        private readonly IFileRepository _repository;

        public FileService(IFileRepository repository)
        {
            _repository = repository;
            CreateFolder();
        }

        public async Task<IList<FileDto>> GetAllAsync(CancellationToken token)
        {
            var fileDto = new List<FileDto>();
            var allPhotos = await _repository.GetAllAsync(token);

            foreach (var photo in allPhotos)
            {
                var bufferImage = await File.ReadAllBytesAsync(photo.PhotoPath, token);

                fileDto.Add(new FileDto
                {
                    Id = photo.Id,
                    Name = photo.Name,
                    DateTime = photo.DateTime,
                    Photo = bufferImage
                });
            }

            return fileDto;
        }

        public async Task AddAsync(FileDto file, CancellationToken token)
        {
            SaveImage(file);

            await _repository.AddAsync(new FileEntity
            {
                Name = file.Name,
                DateTime = file.DateTime,
                PhotoPath = $"{PhotoDataPath}\\{file.Name}"
            }, token);
        }

        public async Task<FileDto> GetByIdAsync(long id, CancellationToken token)
        {
            var result = await _repository.GetByIdAsync(id, token);

            var bufferImage = await File.ReadAllBytesAsync(result.PhotoPath, token);

            return new FileDto
            {
                Id = result.Id,
                Name = result.Name,
                DateTime = result.DateTime,
                Photo = bufferImage
            };
        }

        private static void SaveImage(FileDto file)
        {
            try
            {
                using (var memoryStream = new MemoryStream(file.Photo))
                {
                    var image = Image.FromStream(memoryStream);
                    image.Save($"{file.Name}", ImageFormat.Png);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error while saving image file into server folder!", e);
            }
        }

        private static void CreateFolder()
        {
            try
            {
                if (!Directory.Exists(PhotoDataPath))
                {
                    Directory.CreateDirectory(PhotoDataPath);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error while creating the folder", e);
            }
        }
    }
}