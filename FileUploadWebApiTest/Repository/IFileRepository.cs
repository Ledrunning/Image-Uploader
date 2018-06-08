using FileUploadWebApiTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadWebApiTest.Repository
{
    public interface IFileRepository
    {
        IQueryable<FileModel> GetFiles();
        FileModel GetFilesById(Guid id);
        FileModel AddFile(FileModel file);
        void DeleteFile(Guid id);
    }
}
