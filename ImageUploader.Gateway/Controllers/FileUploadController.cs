using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImageUploader.Gateway.Contracts;
using ImageUploader.Gateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace ImageUploader.Gateway.Controllers
{
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        private readonly IFileService _fileService;

        public FileUploadController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [Route(nameof(Index))]
        public IActionResult Index()
        {
            return Content("Welcome to Image Uploader web api server!");
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IList<ShortFileDto>> GetAll(CancellationToken token)
        {
            return await _fileService.GetAllAsync(token); 
        }

        [HttpGet]
        [Route(nameof(GetById))]
        public async Task<IActionResult> GetById(long id, CancellationToken token)
        {
            var file = await _fileService.GetByIdAsync(id, token);
            if (file == null)
            {
                return NotFound();
            }

            return new ObjectResult(file);
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create([FromBody] FileDto file, CancellationToken token)
        {
            if (file == null)
            {
                return NotFound();
            }

            await _fileService.AddAsync(file, token);
            return new NoContentResult();
        }

        [HttpDelete]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete(long id, CancellationToken token)
        {
            var file = await _fileService.GetByIdAsync(id, token);
            if (file == null)
            {
                return NotFound();
            }

            await _fileService.DeleteAsync(file, token);
            return new NoContentResult();
        }

        [HttpPut]
        [Route(nameof(Update))]
        public async Task<IActionResult> Update(FileDto file, CancellationToken token)
        {
            if (file == null)
            {
                return BadRequest();
            }

            await _fileService.UpdateAsync(file, token);

            return new NoContentResult();
        }
    }
}