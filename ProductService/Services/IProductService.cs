using BasketService.MessagingBus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductService.Domain.Entities;
using ProductService.Infrastructure;
using ProductService.MessageBus;
using RabbitMQ.Client;

namespace ProductService.Services;

public interface IProductService
{
    Task<Guid> AddProduct(ProductModel model);
    Task<string> UpdateProduct(Guid id, string name);
    Task<List<ProductModel>> GetAllProduct();
    Task<ProductModel> GetProductBy(Guid ProductId);
}


public class ProductService : IProductService
{
    private readonly ProductDataBaseContext _context;
    private readonly IMessageRabbitHelper _messageRabbitHelper;
    private readonly RabbitMqConfiguration _rabbitMqConfiguration;
    public ProductService(ProductDataBaseContext context, IMessageRabbitHelper messageRabbitHelper, IOptions<RabbitMqConfiguration> rabbitMqConfiguration)
    {
        _context = context;
        _messageRabbitHelper = messageRabbitHelper;
        _rabbitMqConfiguration = rabbitMqConfiguration.Value;
    }

    public async Task<Guid> AddProduct(ProductModel model)
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
        return product.Id;
    }

    public async Task<string> UpdateProduct(Guid id, string name)
    {
        var product = await _context.Products.FindAsync(id);
        product.EditName(name);
        var result = await _context.SaveChangesAsync();

        #region sendMessageToRebbiteMq

        if (result == 1)
        {
            var connection = await _messageRabbitHelper.CheckCreateRabbitMqConnection(_rabbitMqConfiguration.HostName,
                _rabbitMqConfiguration.UserName, _rabbitMqConfiguration.Password);

            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "ProductUpdated", ExchangeType.Topic, true, false, null);

            var message = new ProductUpdateMessage()
            {
                MessageId = Guid.NewGuid(),
                ProductId = product.Id,
                ProductName = product.Name,
                MessageData = DateTime.Now
            };
            var body = _messageRabbitHelper.CreateBody(message);
            var prop = channel.CreateBasicProperties();
            prop.Persistent = true;
            channel.BasicPublish(exchange: "ProductUpdated", "Product.Updated", prop, body);

        }

        #endregion


        return product.Name;
    }

    public async Task<List<ProductModel>> GetAllProduct()
    {
        return await _context.Products.AsNoTracking()
            .Select(c => new ProductModel()
            {
                Id = c.Id,
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