using MatchPoint.API.Models.Domain;
using MatchPoint.API.Models.DTO.Image;
using MatchPoint.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace MatchPoint.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        // GET: {apibaseurl}/api/images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetAll();

            if (!images.Any() || images is null)
                return NotFound();

            // map to dto
            var response = images.Select(i => new BlogImageDto
            {
                Id = i.Id,
                FileName = i.FileName,
                FileExtension = i.FileExtension,
                Title = i.Title,
                Url = i.Url,
                DateCreated = i.DateCreated,
            }).ToList();

            return Ok(response);
        }


        // POST: {apibaseurl}/api/images
        [HttpPost]
        public async Task<IActionResult> UploadImage(
            [FromForm] IFormFile file, 
            [FromForm] string fileName, 
            [FromForm] string title)
        {
            ValidateFileUpload(file);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var blogImage = new BlogImage
            {
                FileExtension = Path.GetExtension(file.FileName).ToLower(),
                FileName = fileName,
                Title = title,
                DateCreated = DateTime.Now
            };

            blogImage = await _imageRepository.Upload(file,blogImage);

            // Convert Domain Model to DTO
            var blogImageDto = new BlogImageDto
            {
                Id = blogImage.Id,
                Title = blogImage.Title,
                DateCreated = blogImage.DateCreated,
                FileExtension = blogImage.FileExtension,
                FileName = blogImage.FileName,
                Url = blogImage.Url
            };

            return Ok(blogImageDto);
            

        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                ModelState.AddModelError("file", "Unsupported file format");

            if(file.Length > 10485760)
                ModelState.AddModelError("file", "File size cannot be more than 10MB");

        }

    }
}
