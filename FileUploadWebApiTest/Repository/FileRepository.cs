using System;
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
            try
            {
                await _fileContext.Files.AddAsync(file, token);
                await _fileContext.SaveChangesAsync(token);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

        }

        public void DeleteAsync(long id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<FileEntity>> GetAllAsync(CancellationToken token)
        {
            try
            {
                // Files - название таблицы DBSet
                return await _fileContext.Files.AsQueryable().ToListAsync(token);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<FileEntity> GetByIdAsync(long id, CancellationToken token)
        {
            return await _fileContext.Files.SingleOrDefaultAsync(c => c.Id == id, cancellationToken: token);
        }
    }
}