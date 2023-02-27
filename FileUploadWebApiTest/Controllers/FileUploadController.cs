using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FileUploadWebApiTest.Contracts;
using FileUploadWebApiTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadWebApiTest.Controllers
{
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        private readonly IFileService _fileService;

        public FileUploadController(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<IList<FileDto>> GetAll(CancellationToken token)
        {
            return await _fileService.GetAllAsync(token);
        }

        [HttpGet("{id}", Name = "GetFileUpload")]
        public IActionResult GetById(long id, CancellationToken token)
        {
            var file = _fileService.GetByIdAsync(id, token);
            if (file == null)
            {
                return NotFound();
            }

            return new ObjectResult(file);
        }

        [HttpPost]
        public IActionResult Create([FromBody] FileDto file, CancellationToken token)
        {
            if (file == null)
            {
                return BadRequest();
            }

            _fileService.AddAsync(file, token);
            return CreatedAtRoute("GetFileUpload", new { id = file.Id }, file);
        }

        //TODO Under construction
        [HttpDelete("{id}")]
        public IActionResult Delete(long id, CancellationToken token)
        {
            var file = _fileService.GetByIdAsync(id, token);
            if (file == null)
            {
                return NotFound();
            }

            //_fileService.DeleteAsync(id);
            return new NoContentResult();
        }
    }
}