using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FileUploadWebApiTest.Repository.Entity;

namespace FileUploadWebApiTest.Contracts
{
    public interface IFileRepository
    {
        Task<IList<FileEntity>> GetAllAsync(CancellationToken token);
        Task<FileEntity> GetByIdAsync(long id, CancellationToken token);
        Task AddAsync(FileEntity file, CancellationToken token);
        void DeleteAsync(long id, CancellationToken token);
    }
}