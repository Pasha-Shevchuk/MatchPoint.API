using MatchPoint.API.Data;
using MatchPoint.API.Models.Domain;
using MatchPoint.API.Models.DTO.BlogPost;
using MatchPoint.API.Repositories.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatchPoint.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(BlogPost category)
        {
            await _context.BlogPosts.AddAsync(category);
            var result = await _context.SaveChangesAsync();
            return result > 0;

        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var result = await _context.BlogPosts.FirstOrDefaultAsync(c => c.Id == id);
            if(result == null)
                return false;
            _context.BlogPosts.Remove(result);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            var posts = await _context.BlogPosts.Include(x => x.Categories).ToListAsync();
            if (posts == null || !posts.Any())
                return Enumerable.Empty<BlogPost>();

           
            return posts;
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            var post = await _context.BlogPosts
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(c => c.Id == id);

            return post;
        }


        public async Task<bool> UpdateAsync(BlogPost category)
        {
            var existingPost = await _context.BlogPosts.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (existingPost == null)
                return false;
            existingPost.Title = category.Title;
            existingPost.ShortDescription = category.ShortDescription;
            existingPost.Content = category.Content;
            existingPost.FeaturedImageUrl = category.FeaturedImageUrl;
            existingPost.UrlHandle = category.UrlHandle;
            existingPost.PublishedDate = category.PublishedDate;
            existingPost.Author = category.Author;
            existingPost.IsVisible = category.IsVisible;
            _context.BlogPosts.Update(existingPost);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
