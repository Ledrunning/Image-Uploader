using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImageUploader.Gateway.Models;

namespace ImageUploader.Gateway.Contracts
{
    public interface IFileService
    {
        Task<IList<ShortFileDto>> GetAllAsync(CancellationToken token);
        Task AddAsync(FileDto file, CancellationToken token);
        Task<FileDto> GetByIdAsync(long id, CancellationToken token);
        Task UpdateAsync(FileDto file, CancellationToken token);
        Task DeleteAsync(FileDto file, CancellationToken token);
    }
}