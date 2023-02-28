using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileUploadWebApiTest.Contracts;
using FileUploadWebApiTest.Models;
using FileUploadWebApiTest.Repository.Entity;
using Microsoft.EntityFrameworkCore;

namespace FileUploadWebApiTest.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly FileModelContext _fileContext;

        public FileRepository(FileModelContext fileContext)
        {
            _fileContext = fileContext;
        }

        public async Task AddAsync(FileEntity file, CancellationToken token)
        {
            await _fileContext.Files.AddAsync(file, token);
            await _fileContext.SaveChangesAsync(token);
        }

        public async Task DeleteAsync(FileEntity file, CancellationToken token)
        {
            _fileContext.Files.Attach(file);
            _fileContext.Files.Remove(file);
            await _fileContext.SaveChangesAsync(token);
        }

        public async Task<IList<FileEntity>> GetAllAsync(CancellationToken token)
        {
            return await _fileContext.Files.AsQueryable().ToListAsync(token);
        }

        public async Task<FileEntity> GetByIdAsync(long id, CancellationToken token)
        {
            return await _fileContext.Files.SingleOrDefaultAsync(c => c.Id == id, token);
        }
    }
}