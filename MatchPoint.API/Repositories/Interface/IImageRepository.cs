using MatchPoint.API.Models.Domain;

namespace MatchPoint.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
    }
}
