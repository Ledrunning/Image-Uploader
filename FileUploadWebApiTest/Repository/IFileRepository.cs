using System;
using System.Linq;
using FileUploadWebApiTest.Models;

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