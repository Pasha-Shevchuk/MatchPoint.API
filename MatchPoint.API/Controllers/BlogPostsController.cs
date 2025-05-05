using MatchPoint.API.Models.Domain;
using MatchPoint.API.Models.DTO.BlogPost;
using MatchPoint.API.Models.DTO.Category;
using MatchPoint.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatchPoint.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
        }


        // get: -> dto
        [HttpGet]
        public async Task<IActionResult> GetBlogPosts()
        {
            var blogPosts = await _blogPostRepository.GetAllAsync();

            if (blogPosts == null || !blogPosts.Any())
                return NotFound("No blog posts found");

            var blogPostDto = blogPosts.Select(b => new GetBlogPostDto
            {
                Id = b.Id,
                Title = b.Title,
                ShortDescription = b.ShortDescription,
                Content = b.Content,
                FeaturedImageUrl = b.FeaturedImageUrl,
                UrlHandle = b.UrlHandle,
                PublishedDate = b.PublishedDate,
                Author = b.Author,
                IsVisible = b.IsVisible,
                Categories = b.Categories.Select(x => new GetCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            });

            return Ok(blogPostDto);
        }


        // get: id -> dto
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById(Guid id)
        {
            var blogPost = await _blogPostRepository.GetByIdAsync(id);
            if(blogPost == null)
                return NotFound("Blog post not found");
            var blogPostDto = new GetBlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories?.Select(x => new GetCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList() ?? new List<GetCategoryDto>()

            };
            return Ok(blogPostDto);
        }

        // get: blobdost by urlhandle
        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            // Get blogpost details
            var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
            if (blogPost is null) 
                return NotFound();

            var blogPostDto = new GetBlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories?.Select(x => new GetCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList() ?? new List<GetCategoryDto>()

            };

            return Ok(blogPostDto);
        }



        // post: model -> b
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostDto request)
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
                IsVisible = request.IsVisible,
                Categories = new List<Category>()
            };

            // fill categories for blog posts
            foreach(var categoryGuid in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(categoryGuid);
                if(existingCategory != null)
                    blogpost.Categories.Add(existingCategory);
            }

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
                IsVisible = blogpost.IsVisible,

                Categories = blogpost.Categories.Select(x => new GetCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };

            return Ok(blodpostDto);
        }

        // put: id, model -> b
        [HttpPut("{id}")]
        [Authorize(Roles = "Writer")]
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
                IsVisible = request.IsVisible,
                Categories = new List<Category>()
            };

            foreach(var categoryGuid in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(categoryGuid);   

                if(existingCategory != null)
                    blogPost.Categories.Add(existingCategory);
            } 

            // Call repository to update BlogPost Domain Model

            var result = await _blogPostRepository.UpdateAsync(blogPost);
            if (result == false)
                return BadRequest("Failed to update blog post");


            // convert blog post to dto
            var blogPostDto = new GetBlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                ShortDescription = blogPost.ShortDescription,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                PublishedDate = blogPost.PublishedDate,
                Author = blogPost.Author,
                IsVisible = blogPost.IsVisible,
                Categories = blogPost.Categories.Select(x => new GetCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };


            return Ok(blogPostDto);

        }

        // delete: id -> b
        [HttpDelete("{id}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlogPost(Guid id)
        {
            var result = await _blogPostRepository.DeleteAsync(id);
            if (result == false)
                return BadRequest("Failed to delete blog post");
            return NoContent();
        }

    }
}
