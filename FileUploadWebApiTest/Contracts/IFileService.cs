using FileUploadWebApiTest.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadWebApiTest.Contracts
{
    public interface IFileService
    {
        Task<IList<FileDto>> GetAllAsync(CancellationToken token);
        Task AddAsync(FileDto file, CancellationToken token);
        Task<FileDto> GetByIdAsync(long id, CancellationToken token);
        Task DeleteAsync(FileDto file, string path, CancellationToken token);
    }
}