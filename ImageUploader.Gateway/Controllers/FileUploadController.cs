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
        public async Task<IList<ShortImageDto>> GetAll(CancellationToken token)
        {
            return await _fileService.GetAllAsync(token);
        }

        [HttpGet]
        [Route(nameof(GetById))]
        public async Task<IActionResult> GetById(long id, CancellationToken token)
        {
            var imageDto = await _fileService.GetByIdAsync(id, token);
            if (imageDto == null)
            {
                return NotFound();
            }

            return new ObjectResult(imageDto);
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create([FromBody] ImageDto imageDto, CancellationToken token)
        {
            if (imageDto == null)
            {
                return NotFound();
            }

            await _fileService.AddAsync(imageDto, token);
            return new NoContentResult();
        }

        [HttpDelete]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete(long id, CancellationToken token)
        {
            var imageDto = await _fileService.GetByIdAsync(id, token);
            if (imageDto == null)
            {
                return NotFound();
            }

            await _fileService.DeleteAsync(imageDto, token);
            return new NoContentResult();
        }

        [HttpPost]
        [Route(nameof(Update))]
        public async Task<IActionResult> Update([FromBody] ImageDto imageDto, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _fileService.UpdateAsync(imageDto, token);

            return new NoContentResult();
        }
    }
}