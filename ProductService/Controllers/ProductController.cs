using Microsoft.AspNetCore.Mvc;
using ProductService.Services;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{

    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProduct([FromForm] ProductModel model)
    {
        var result = await _productService.AddProduct(new ProductModel()
        {
            Price = model.Price,
            CategoryId = model.CategoryId,
            CategoryName = model.CategoryName,
            Description = model.Description,
            Image = model.Image,
            Name = model.Name
        });

        return Created($"/api/Product/GetProductBy/{result}", result);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> AllProduct()
    {
        var result = await _productService.GetAllProduct();
        return Ok(result);
    }

    [HttpGet("GetProductBy/{id}")]
    public async Task<IActionResult> GetProductBy(Guid id)
    {
        var result = await _productService.GetProductBy(id);
        return Ok(result);
    }
}