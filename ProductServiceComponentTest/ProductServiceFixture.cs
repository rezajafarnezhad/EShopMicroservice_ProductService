using BasketService.MessagingBus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductService.Infrastructure;
using ProductService.MessageBus;
using ProductService.Services;

namespace ProductServiceComponentTest;

public class ProductServiceFixture
{
    public IProductService productService { get; }
    public ICategoryService categoryService { get; }
    public IMessageRabbitHelper RabbitHelper { get; }
    public ProductDataBaseContext dataBaseContext { get; }
    private IOptions<RabbitMqConfiguration> _rabbitMqConfiguration;
    public ProductServiceFixture()
    {

        var builder = new DbContextOptionsBuilder<ProductDataBaseContext>();
        builder.UseInMemoryDatabase("ProductDateBaseTest");
        dataBaseContext = new ProductDataBaseContext(builder.Options);
        RabbitHelper = new RabbitMqMessageBusHelper();
        _rabbitMqConfiguration = Options.Create(new RabbitMqConfiguration());
        productService = new ProductService.Services.ProductService(dataBaseContext, RabbitHelper, _rabbitMqConfiguration);
        categoryService = new CategoryService(dataBaseContext);
    }
}