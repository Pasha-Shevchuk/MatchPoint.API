using MatchPoint.API.Data;
using MatchPoint.API.Models.Domain;
using MatchPoint.API.Repositories.Interface;

namespace MatchPoint.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            return await _context.SaveChangesAsync() > 0;
        }


    }
}
