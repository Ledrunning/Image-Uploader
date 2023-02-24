using System;
using System.Linq;
using FileUploadWebApiTest.Models;
using FileUploadWebApiTest.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadWebApiTest.Controllers
{
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        private readonly IFileRepository _fileRepository;

        public FileUploadController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public IQueryable<FileModel> GetAll()
        {
            return _fileRepository.GetFiles();
        }

        [HttpGet("{id}", Name = "GetFileUpload")]
        public IActionResult GetById(Guid id)
        {
            var file = _fileRepository.GetFilesById(id);
            if (file == null)
            {
                return NotFound();
            }

            return new ObjectResult(file);
        }

        [HttpPost]
        public IActionResult Create([FromBody] FileModel file)
        {
            if (file == null)
            {
                return BadRequest();
            }

            _fileRepository.AddFile(file);
            return CreatedAtRoute("GetFileUpload", new { id = file.Id }, file);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var file = _fileRepository.GetFilesById(id);
            if (file == null)
            {
                return NotFound();
            }

            _fileRepository.DeleteFile(id);
            return new NoContentResult();
        }
    }
}