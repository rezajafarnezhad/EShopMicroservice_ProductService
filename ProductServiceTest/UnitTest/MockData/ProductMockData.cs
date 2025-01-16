using ProductService.Services;
using Tynamix.ObjectFiller;

namespace ProductServiceTest.UnitTest.MockData;

public class ProductMockData
{
    List<ProductModel> products = new();
    public Task<List<ProductModel>> GetAllProducts()
    {
        products.AddRange(new Filler<ProductModel>().Create(20));
        return Task.FromResult(products);
    }

    public Task<ProductModel> GetByIdProducts(Guid id)
    {
        products.AddRange(new Filler<ProductModel>().Create(20));
        id = products[6].Id;
        var result = products.SingleOrDefault(c => c.Id == id);
        return Task.FromResult(result);
    }
}