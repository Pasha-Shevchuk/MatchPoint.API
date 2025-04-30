using MatchPoint.API.Models.Domain;

namespace MatchPoint.API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<bool> CreateAsync(BlogPost category);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(BlogPost category);
        Task<bool> DeleteAsync(Guid id);
    }
}
