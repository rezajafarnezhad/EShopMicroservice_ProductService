using Microsoft.AspNetCore.Mvc;
using ProductService.Services;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PanelController : ControllerBase
{
    private readonly IProductService _productService;
    public PanelController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("CreateProduct")]
    public async Task<IActionResult> CreateProduct([FromForm] ProductModel model)
    {
        var result = _productService.AddProduct(new ProductModel()
        {
            Price = model.Price,
            CategoryId = model.CategoryId,
            CategoryName = model.CategoryName,
            Description = model.Description,
            Image = model.Image,
            Name = model.Name
        });

        return NoContent();
    }


    [HttpPut("UpdateProduct")]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductModel model)
    {
        await _productService.UpdateProduct(model.Id, model.Name);
        return NoContent();
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

public class UpdateProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
