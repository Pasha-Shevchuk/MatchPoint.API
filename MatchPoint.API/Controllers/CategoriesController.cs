using MatchPoint.API.Data;
using MatchPoint.API.Models.Domain;
using MatchPoint.API.Models.DTO;
using MatchPoint.API.Repositories.Interface;
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

        [HttpPost]
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
