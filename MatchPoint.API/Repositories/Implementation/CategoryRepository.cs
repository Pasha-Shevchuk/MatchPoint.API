using MatchPoint.API.Data;
using MatchPoint.API.Models.Domain;
using MatchPoint.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(category is null)
                return false;

            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string? query = null, string? sortBy = null, string? sortDirection = null)
        {
            // query the database
            var categories = _context.Categories.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(query))
            {
                categories = categories.Where(x => x.Name.Contains(query));
            }

            // Sorting
            if(!string.IsNullOrEmpty(sortBy))
            {
                if(string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                    if(isAsc)
                        categories = categories.OrderBy(x => x.Name);
                    else
                        categories = categories.OrderByDescending(x => x.Name);
                }
                if (string.Equals(sortBy, "UrlHandle", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);
                    if (isAsc)
                        categories = categories.OrderBy(x => x.UrlHandle);
                    else
                        categories = categories.OrderByDescending(x => x.UrlHandle);
                }
            }

            // Pagination


            return await categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
            return category;
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            var categoryFromDB = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);    
            if(categoryFromDB is null)
                return false;

            _context.Entry(categoryFromDB).CurrentValues.SetValues(category);
            return await _context.SaveChangesAsync() > 0;
            
        }
    }
}
