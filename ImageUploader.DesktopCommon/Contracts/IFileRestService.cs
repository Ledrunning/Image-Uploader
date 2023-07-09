using ImageUploader.DesktopCommon.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ImageUploader.DesktopCommon.Contracts
{
    public interface IFileRestService
    {
        Task<IList<ImageModel>> GetAllDataFromFilesAsync();
        Task<ImageDto> GetFileAsync(long id);
        Task AddFileAsync(ImageDto fileModel);
        Task DeleteAsync(long id);
        Task UpdateAsync(ImageDto fileModel);
    }
}