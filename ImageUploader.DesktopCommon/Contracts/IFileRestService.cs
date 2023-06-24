using ImageUploader.DesktopCommon.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ImageUploader.DesktopCommon.Contracts
{
    public interface IFileRestService
    {
        Task<IList<FileModel>> GetAllDataFromFilesAsync();
        Task<FileDto> GetFileAsync(long id);
        Task AddFileAsync(FileDto fileModel);
        Task DeleteAsync(long id);
        Task UpdateAsync(FileDto fileModel);
    }
}