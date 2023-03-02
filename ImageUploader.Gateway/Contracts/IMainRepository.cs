using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImageUploader.Gateway.Repository.Entity;

namespace ImageUploader.Gateway.Contracts
{
    public interface IMainRepository<T> where T : BaseEntity, new()
    {
        Task<IList<T>> GetAllAsync(CancellationToken token);
        Task<T> GetByIdAsync(long id, CancellationToken token);
        Task<T> AddAsync(T entity, CancellationToken token);
        Task UpdateAsync(T entity, CancellationToken token);
        Task DeleteAsync(T entity, CancellationToken token);
    }
}