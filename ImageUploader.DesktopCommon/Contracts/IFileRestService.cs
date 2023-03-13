using ImageUploader.DesktopCommon.Models;
using System.Threading.Tasks;

namespace ImageUploader.DesktopCommon.Contracts
{
    public interface IFileRestService
    {
        Task<FileModel> GetFileAsync(long id);
        Task AddFileAsync(FileModel fileModel);
        Task DeleteAsync(long id);
        Task UpdateAsync(FileModel fileModel);
    }
}