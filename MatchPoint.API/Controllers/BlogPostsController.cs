using MatchPoint.API.Models.Domain;
using MatchPoint.API.Models.DTO.BlogPost;
using MatchPoint.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchPoint.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }


        // get: -> dto
        [HttpGet]
        public async Task<IActionResult> GetBlogPosts()
        {
            var blogPosts = await _blogPostRepository.GetAllAsync();

            if (blogPosts == null || !blogPosts.Any())
                return NotFound("No blog posts found");

            var blogPostDto = blogPosts.Select(c => new GetBlogPostDto
            {
                Id = c.Id,
                Title = c.Title,
                ShortDescription = c.ShortDescription,
                Content = c.Content,
                FeaturedImageUrl = c.FeaturedImageUrl,
                UrlHandle = c.UrlHandle,
                PublishedDate = c.PublishedDate,
                Author = c.Author,
                IsVisible = c.IsVisible
            });

            return Ok(blogPostDto);
        }


        // get: id -> dto
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogPostById(Guid id)
        {
            var catregory = await _blogPostRepository.GetByIdAsync(id);
            if(catregory == null)
                return NotFound("Blog post not found");
            var blogPostDto = new GetBlogPostDto
            {
                Id = catregory.Id,
                Title = catregory.Title,
                ShortDescription = catregory.ShortDescription,
                Content = catregory.Content,
                FeaturedImageUrl = catregory.FeaturedImageUrl,
                UrlHandle = catregory.UrlHandle,
                PublishedDate = catregory.PublishedDate,
                Author = catregory.Author,
                IsVisible = catregory.IsVisible
            };
            return Ok(blogPostDto);
        }

        // post: model -> b
        [HttpPost]
        public  async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostDto request)
        {
            var blogpost = new BlogPost
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Author = request.Author,
                IsVisible = request.IsVisible
            };

            var result =  await _blogPostRepository.CreateAsync(blogpost);
            if (result == false)
                return BadRequest("Failed to create blog post");

            // convert blog post to dto

            var blodpostDto = new GetBlogPostDto
            {
                Id = blogpost.Id,
                Title = blogpost.Title,
                ShortDescription = blogpost.ShortDescription,
                Content = blogpost.Content,
                FeaturedImageUrl = blogpost.FeaturedImageUrl,
                UrlHandle = blogpost.UrlHandle,
                PublishedDate = blogpost.PublishedDate,
                Author = blogpost.Author,
                IsVisible = blogpost.IsVisible
            };

            return Ok(blodpostDto);
        }

        // put: id, model -> b
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogPost(Guid id, [FromBody] UpdateBlogPostDto request)
        {
            var blogPost = new BlogPost
            {
                Id = id,
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Author = request.Author,
                IsVisible = request.IsVisible
            };

            var result = await _blogPostRepository.UpdateAsync(blogPost);
            if (result == false)
                return BadRequest("Failed to update blog post");
            return Ok(request);

        }

        // delete: id -> b
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(Guid id)
        {
            var result = await _blogPostRepository.DeleteAsync(id);
            if (result == false)
                return BadRequest("Failed to delete blog post");
            return Ok("Blog post deleted successfully");
        }

    }
}
