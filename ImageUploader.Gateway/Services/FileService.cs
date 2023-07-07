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
        private readonly IMainRepository<ImageEntity> _repository;

        public FileService(IMainRepository<ImageEntity> repository, ILogger<FileService> logger)
        {
            _repository = repository;
            _logger = logger;
            CreateFolder();
        }

        public async Task<IList<ShortImageDto>> GetAllAsync(CancellationToken token)
        {
            try
            {
                var allImages = await _repository.GetAllAsync(token);

                _logger.LogInformation("Getting all images from server");

                return allImages.Select(image => new ShortImageDto
                {
                    Id = image.Id, Name = image.Name, DateTime = image.DateTime, CreationTime = image.CreationTime,
                    FileSize = image.FileSize
                }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get all images from server! Error: {e}", e);
                throw new ImageUploaderException("Failed to get all images from server!", e);
            }
        }

        public async Task AddAsync(ImageDto imageDto, CancellationToken token)
        {
            SaveImage(imageDto);

            try
            {
                await _repository.AddAsync(new ImageEntity
                {
                    Name = imageDto.Name,
                    DateTime = imageDto.DateTime,
                    CreationTime = imageDto.CreationTime,
                    FileSize = imageDto.FileSize,
                    PhotoPath = $"{PhotoDataPath}\\{imageDto.Name}"
                }, token);

                _logger.LogInformation("The image and details has been added on the server");
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to add the image imageDto! Error: {e}", e);
                throw new ImageUploaderException("Failed to add the image imageDto!", e);
            }
        }

        public async Task<ImageDto> GetByIdAsync(long id, CancellationToken token)
        {
            try
            {
                var imageEntity = await _repository.GetByIdAsync(id, token);

                var bufferImage = await File.ReadAllBytesAsync(imageEntity.PhotoPath, token);

                _logger.LogInformation("Getting the image from the server");

                return new ImageDto
                {
                    Id = imageEntity.Id,
                    Name = imageEntity.Name,
                    DateTime = imageEntity.DateTime,
                    CreationTime = imageEntity.CreationTime,
                    FileSize = imageEntity.FileSize,
                    Photo = bufferImage,
                    PhotoPath = imageEntity.PhotoPath
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to getting the image by Id! Error: {e}", e);
                throw new ImageUploaderException("Failed to getting the image by Id!", e);
            }
        }

        public async Task UpdateAsync(ImageDto file, CancellationToken token)
        {
            try
            {
                var fileEntity = new ImageEntity
                {
                    Id = file.Id,
                    Name = file.Name,
                    DateTime = file.DateTime,
                    CreationTime = file.CreationTime,
                    FileSize = file.FileSize,
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

        public async Task DeleteAsync(ImageDto file, CancellationToken token)
        {
            var fileEntity = new ImageEntity
            {
                Id = file.Id,
                Name = file.Name,
                DateTime = file.DateTime,
                CreationTime = file.CreationTime,
                FileSize = file.FileSize,
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
                _logger.LogError("Error while deleting image imageDto in the server folder! Error: {e}", e);
                throw new ImageUploaderException("Error while deleting image imageDto in the server folder!", e);
            }
        }

        private void SaveImage(ImageDto file)
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
                _logger.LogError("Error while saving image imageDto into server folder! Error: {e}", e);
                throw new ImageUploaderException("Error while saving image imageDto into server folder!", e);
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
                _logger.LogError("Error while saving image imageDto into server folder! Error: {e}", e);
                throw new ImageUploaderException("Error while creating the image folder", e);
            }
        }
    }
}