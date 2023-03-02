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
    public class MainRepository<T> : IMainRepository<T> where T : BaseEntity, new()
    {
        private readonly MainDatabaseContext _mainContext;

        public MainRepository(MainDatabaseContext mainContext)
        {
            _mainContext = mainContext;
        }

        public async Task<T> AddAsync(T entity, CancellationToken token)
        {
            await _mainContext.Set<T>().AddAsync(entity, token);
            await _mainContext.SaveChangesAsync(token);

            return entity;
        }

        public async Task DeleteAsync(T entity, CancellationToken token)
        {
            _mainContext.Set<T>().Remove(entity);
            await _mainContext.SaveChangesAsync(token);
        }

        public async Task<IList<T>> GetAllAsync(CancellationToken token)
        {
            return await _mainContext.Set<T>().AsNoTracking().AsQueryable().ToListAsync(token);
        }

        public async Task<T> GetByIdAsync(long id, CancellationToken token)
        {
            return await _mainContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, cancellationToken: token);
        }

        public async Task UpdateAsync(T entity, CancellationToken token)
        {
            _mainContext.Entry(entity).State = EntityState.Modified;
            await _mainContext.SaveChangesAsync(token);
        }
    }
}