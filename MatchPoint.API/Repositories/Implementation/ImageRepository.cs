using MatchPoint.API.Data;
using MatchPoint.API.Models.Domain;
using MatchPoint.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace MatchPoint.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public ImageRepository(
            IWebHostEnvironment webHostEnvironment, 
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext _context)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this._context = _context;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            return await _context.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            // Upload Image to API/Images
            var localPath = Path.Combine(
                webHostEnvironment.ContentRootPath, 
                "Images", 
                $"{blogImage.FileName}{blogImage.FileExtension}");

            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);


            // Update database
            // https://projname.com/images/image.jpg
            var httpRequest = httpContextAccessor.HttpContext.Request;

            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}" +
                $"/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

            await _context.BlogImages.AddAsync(blogImage);
            await _context.SaveChangesAsync();
            return blogImage;

        }
    }
}
