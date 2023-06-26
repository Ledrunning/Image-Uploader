using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ImageUploader.Gateway.Contracts;
using ImageUploader.Gateway.Exceptions;
using ImageUploader.Gateway.Models;
using ImageUploader.Gateway.Repository.Entity;
using Microsoft.Extensions.Logging;

namespace ImageUploader.Gateway.Services
{
    public class FileService : IFileService
    {
        private const string FolderName = "ImageFolder";

        private static readonly string PhotoDataPath = Path.GetDirectoryName(
                                                           Assembly.GetExecutingAssembly().Location) +
                                                       $"\\{FolderName}";

        private readonly ILogger<FileService> _logger;

        private readonly IMainRepository<FileEntity> _repository;

        public FileService(IMainRepository<FileEntity> repository, ILogger<FileService> logger)
        {
            _repository = repository;
            _logger = logger;
            CreateFolder();
        }

        public async Task<IList<ShortFileDto>> GetAllAsync(CancellationToken token)
        {
            try
            {
                var allPhotos = await _repository.GetAllAsync(token);

                _logger.LogInformation("Getting all images from server");

                return allPhotos.Select(photo => new ShortFileDto
                    { Id = photo.Id, Name = photo.Name, DateTime = photo.DateTime }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get all images from server! Error: {e}", e);
                throw new ImageUploaderException("Failed to get all images from server!", e);
            }
        }

        public async Task AddAsync(FileDto file, CancellationToken token)
        {
            SaveImage(file);

            try
            {
                await _repository.AddAsync(new FileEntity
                {
                    Name = file.Name,
                    DateTime = file.DateTime,
                    PhotoPath = $"{PhotoDataPath}\\{file.Name}"
                }, token);

                _logger.LogInformation("The image and details has been added on the server");
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to add the image file! Error: {e}", e);
                throw new ImageUploaderException("Failed to add the image file!", e);
            }
        }

        public async Task<FileDto> GetByIdAsync(long id, CancellationToken token)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id, token);

                var bufferImage = await File.ReadAllBytesAsync(result.PhotoPath, token);

                _logger.LogInformation("Getting the image from the server");

                return new FileDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    DateTime = result.DateTime,
                    Photo = bufferImage,
                    PhotoPath = result.PhotoPath
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to getting the image by Id! Error: {e}", e);
                throw new ImageUploaderException("Failed to getting the image by Id!", e);
            }
        }

        public async Task UpdateAsync(FileDto file, CancellationToken token)
        {
            try
            {
                var fileEntity = new FileEntity
                {
                    Id = file.Id,
                    Name = file.Name,
                    DateTime = file.DateTime,
                    PhotoPath = $"{PhotoDataPath}\\{file.Name}"
                };

                if (file.IsUpdated)
                {
                    File.Delete($"{PhotoDataPath}\\{file.LastPhotoName}");
                    SaveImage(file);
                }

                await _repository.UpdateAsync(fileEntity, token);
                _logger.LogInformation("The image has been successfully updated in the server");
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to update the image! Error: {e}", e);
                throw new ImageUploaderException("Failed to update the image!", e);
            }
        }

        public async Task DeleteAsync(FileDto file, CancellationToken token)
        {
            var fileEntity = new FileEntity
            {
                Id = file.Id,
                Name = file.Name,
                DateTime = file.DateTime,
                PhotoPath = file.PhotoPath
            };

            try
            {
                File.Delete($"{PhotoDataPath}\\{file.Name}");
                await _repository.DeleteAsync(fileEntity, token);

                _logger.LogInformation("The image and details have been deleted successfully");
            }
            catch (Exception e)
            {
                _logger.LogError("Error while deleting image file in the server folder! Error: {e}", e);
                throw new ImageUploaderException("Error while deleting image file in the server folder!", e);
            }
        }

        private void SaveImage(FileDto file)
        {
            try
            {
                using (var memoryStream = new MemoryStream(file.Photo))
                {
                    var image = Image.FromStream(memoryStream);
                    image.Save($"{PhotoDataPath}\\{file.Name}", ImageFormat.Png);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while saving image file into server folder! Error: {e}", e);
                throw new ImageUploaderException("Error while saving image file into server folder!", e);
            }
        }

        /// <summary>
        ///     Creating a folder only for the first time the app running
        /// </summary>
        /// <exception cref="ImageUploaderException">Error while creating the image folder</exception>
        private void CreateFolder()
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
                _logger.LogError("Error while saving image file into server folder! Error: {e}", e);
                throw new ImageUploaderException("Error while creating the image folder", e);
            }
        }
    }
}