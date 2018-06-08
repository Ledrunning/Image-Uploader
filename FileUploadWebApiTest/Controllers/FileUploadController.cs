using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileUploadWebApiTest.Models;
using FileUploadWebApiTest.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadWebApiTest.Controllers
{
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        public IFileRepository FileRepository { get; set; }

        public FileUploadController(IFileRepository fileRepository)
        {
            FileRepository = fileRepository;
        }

        public IQueryable<FileModel> GetAll()
        {
            return FileRepository.GetFiles();
        }

        [HttpGet("{id}", Name = "GetFileUpload")]
        public IActionResult GetById(Guid id)
        {
            var file = FileRepository.GetFilesById(id);
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
            FileRepository.AddFile(file);
            return CreatedAtRoute("GetFileUpload", new { id = file.Id }, file);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var file = FileRepository.GetFilesById(id);
            if (file == null)
            {
                return NotFound();
            }

            FileRepository.DeleteFile(id);
            return new NoContentResult();
        }

        #region upload file
        //[HttpPost]
        //public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        //{
        //    if (uploadedFile != null)
        //    {
        //        // путь к папке Files
        //        string path = "/Files/" + uploadedFile.FileName;
        //        // сохраняем файл в папку Files в каталоге wwwroot
        //        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
        //        {
        //            await uploadedFile.CopyToAsync(fileStream);
        //        }
        //        FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
        //        _context.Files.Add(file);
        //        _context.SaveChanges();
        //    }
        //    // Тут же Веп Апи а не МВС. Надо CreatedAt
        //    return RedirectToAction("Index");
        //}
        #endregion
    }
}