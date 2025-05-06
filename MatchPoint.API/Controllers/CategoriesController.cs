using MatchPoint.API.Data;
using MatchPoint.API.Models.Domain;
using MatchPoint.API.Models.DTO.Category;
using MatchPoint.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchPoint.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] string? query)
        {
            var categories = await _categoryRepository.GetAllAsync(query);
            if (categories is null)
                return NotFound("No categories found");

            // map
            //var categoriesDto = categories.Select(c => new GetCategoryDto
            //{
            //    Name = c.Name,
            //}).ToList();
            return Ok(categories.Select(c => new {c.Id, c.Name, c.UrlHandle}));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetCategory([FromRoute]  Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound($"Category with id {id} not found");
            // map
            var categoryMap = new GetCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(categoryMap);
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            
            var success = await _categoryRepository.CreateAsync(category);
            if(!success)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating category");

            return Ok();
        }

        [HttpPut("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto request)
        {
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            bool updatedSuccess = await _categoryRepository.UpdateAsync(category);
            if(!updatedSuccess)
                return NotFound($"Category with id {id} not found");

            return Ok();
        }

        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var deletedSuccess = await _categoryRepository.DeleteAsync(id);
            if (!deletedSuccess)
                return NotFound($"Category with id {id} not found");
            return Ok();

        }
    }
}





























//private readonly ApplicationDbContext _context;
//public CategoriesController(ApplicationDbContext context)
//{
//    _context = context;
//}

//// POST:api/categories
//[HttpPost]
//public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
//{
//    // Map DTO to Domain Model
//    var category = new Category
//    {
//        Name = request.Name,
//        UrlHandle = request.UrlHandle
//    };
//    // create new category in DbSet
//    await _context.Categories.AddAsync(category);
//    // Actually apply changes to db
//    await _context.SaveChangesAsync();

//    // Domain model to DTO for exposion
//    var response = new CategoryDto
//    {
//        Id = category.Id,
//        Name = category.Name,
//        UrlHandle = category.UrlHandle
//    };
//    return Ok(response);
//}
