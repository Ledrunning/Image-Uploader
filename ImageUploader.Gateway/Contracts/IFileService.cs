using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImageUploader.Gateway.Models;

namespace ImageUploader.Gateway.Contracts
{
    public interface IFileService
    {
        Task<IList<ShortImageDto>> GetAllAsync(CancellationToken token);
        Task AddAsync(ImageDto imageDto, CancellationToken token);
        Task<ImageDto> GetByIdAsync(long id, CancellationToken token);
        Task UpdateAsync(ImageDto file, CancellationToken token);
        Task DeleteAsync(ImageDto file, CancellationToken token);
    }
}