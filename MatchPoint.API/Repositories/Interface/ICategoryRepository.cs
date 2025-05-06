using MatchPoint.API.Models.Domain;

namespace MatchPoint.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<bool> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync(string? query = null, string? sortBy = null, string? sortDirection = null);  
        Task<Category?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Category category);
        Task<bool> DeleteAsync(Guid id);
    }
}
