using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageUploader.Gateway.Contracts;
using ImageUploader.Gateway.Models;
using ImageUploader.Gateway.Repository.Entity;
using Microsoft.EntityFrameworkCore;

namespace ImageUploader.Gateway.Repository
{
    public class FileRepository<T> : IFileRepository<T> where T : BaseEntity, new()
    {
        private readonly FileModelContext _fileContext;

        public FileRepository(FileModelContext fileContext)
        {
            _fileContext = fileContext;
        }

        public async Task<T> AddAsync(T entity, CancellationToken token)
        {
            await _fileContext.Set<T>().AddAsync(entity, token);
            await _fileContext.SaveChangesAsync(token);

            return entity;
        }

        public async Task DeleteAsync(T entity, CancellationToken token)
        {
            _fileContext.Set<T>().Remove(entity);
            await _fileContext.SaveChangesAsync(token);
        }

        public async Task<IList<T>> GetAllAsync(CancellationToken token)
        {
            return await _fileContext.Set<T>().AsNoTracking().AsQueryable().ToListAsync(token);
        }

        public async Task<T> GetByIdAsync(long id, CancellationToken token)
        {
            return await _fileContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, cancellationToken: token);
        }

        public async Task UpdateAsync(T entity, CancellationToken token)
        {
            _fileContext.Entry(entity).State = EntityState.Modified;
            await _fileContext.SaveChangesAsync(token);
        }
    }
}