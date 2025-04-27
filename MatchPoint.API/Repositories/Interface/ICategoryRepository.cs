using MatchPoint.API.Models.Domain;

namespace MatchPoint.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<bool> CreateAsync(Category category);
    }
}
