using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Infrastructure;

namespace ProductService.Services;

public interface IProductService
{
    Task AddProduct(ProductModel model);
    Task<List<ProductModel>> GetAllProduct();
    Task<ProductModel> GetProductBy(Guid ProductId);
}


public class ProductService : IProductService
{
    private readonly ProductDataBaseContext _context;
    public ProductService(ProductDataBaseContext context)
    {
        _context = context;
    }

    public async Task AddProduct(ProductModel model)
    {
        var product = new Product()
        {
            Name = model.Name,
            CategoryId = model.CategoryId,
            Image = model.Image,
            Description = model.Description,
            Price = model.Price
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProductModel>> GetAllProduct()
    {
        return await _context.Products.AsNoTracking()
            .Select(c => new ProductModel()
            {
                Description = c.Description,
                Price = c.Price,
                CategoryId = c.CategoryId,
                Image = c.Image,
                Name = c.Name,
                CategoryName = c.Category.Name
            }).ToListAsync();
    }

    public async Task<ProductModel> GetProductBy(Guid ProductId)
    {
        var data = await _context.Products
            .Where(c => c.Id == ProductId)
            .Select(c => new ProductModel()
            {
                Id = c.Id,
                CategoryName = c.Category.Name,
                CategoryId = c.CategoryId,
                Description = c.Description,
                Image = c.Image,
                Name = c.Name,
                Price = c.Price,
            }).SingleOrDefaultAsync();

        if (data is null)
            throw new Exception("Product Not found ..");

        return data;
    }
}


public class ProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
}