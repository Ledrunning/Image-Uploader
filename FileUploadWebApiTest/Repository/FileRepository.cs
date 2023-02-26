﻿using System;
using System.Linq;
using FileUploadWebApiTest.Models;

namespace FileUploadWebApiTest.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly FileModelContext _fileContext;

        public FileRepository(FileModelContext fileContext)
        {
            _fileContext = fileContext;
        }

        public FileModel AddFile(FileModel file)
        {
            try
            {
                _fileContext.Files.Add(file);
                _fileContext.SaveChanges();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

            return file;
        }

        public void DeleteFile(long id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FileModel> GetFiles()
        {
            try
            {
                // Files - название таблицы DBSet
                return _fileContext.Files.AsQueryable();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public FileModel GetFilesById(long id)
        {
            return _fileContext.Files.SingleOrDefault(c => c.Id == id);
        }
    }
}