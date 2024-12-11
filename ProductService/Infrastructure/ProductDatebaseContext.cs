using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure;

public class ProductDataBaseContext : DbContext
{
    public ProductDataBaseContext(DbContextOptions<ProductDataBaseContext> options) : base(options)
    {

    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

}