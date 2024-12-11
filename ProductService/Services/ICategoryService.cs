using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Infrastructure;

namespace ProductService.Services;

public interface ICategoryService
{
    Task AddCategory(string name);
    Task<Dictionary<Guid, string>> GetAllCategories();
}


public class CategoryService : ICategoryService
{
    private readonly ProductDataBaseContext _context;
    public CategoryService(ProductDataBaseContext context)
    {
        _context = context;
    }

    public async Task AddCategory(string name)
    {
        var category = new Category()
        {
            Name = name
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
    }

    public async Task<Dictionary<Guid, string>> GetAllCategories()
    {
        var data = await _context.Categories.AsNoTracking()
            .ToDictionaryAsync(c => c.Id, c => c.Name);
        return data;
    }
}
