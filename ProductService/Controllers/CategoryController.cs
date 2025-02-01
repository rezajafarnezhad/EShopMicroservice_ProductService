using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Services;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{

    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [Authorize("ProductForAdmin")]
    [HttpPost("CreateCategory")]
    public async Task<IActionResult> CreateCategory([FromForm] string name)
    {
        var result = _categoryService.AddCategory(name);
        return Ok();
    }
    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategories();
        return Ok(result);
    }
}