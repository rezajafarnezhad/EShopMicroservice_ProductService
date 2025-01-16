using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Infrastructure;

namespace ProductService.Services;

public interface ICategoryService
{
    Task<Guid> AddCategory(string name);
    Task<Dictionary<Guid, string>> GetAllCategories();
}


public class CategoryService : ICategoryService
{
    private readonly ProductDataBaseContext _context;
    public CategoryService(ProductDataBaseContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddCategory(string name)
    {
        var category = new Category()
        {
            Name = name
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category.Id;
    }

    public async Task<Dictionary<Guid, string>> GetAllCategories()
    {
        var data = await _context.Categories.AsNoTracking()
            .ToDictionaryAsync(c => c.Id, c => c.Name);
        return data;
    }
}
