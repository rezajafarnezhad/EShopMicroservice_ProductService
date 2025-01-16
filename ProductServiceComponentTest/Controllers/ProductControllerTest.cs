using Microsoft.AspNetCore.Mvc;
using ProductService.Controllers;
using ProductService.Services;
using Tynamix.ObjectFiller;

namespace ProductServiceComponentTest.Controllers;

public class ProductControllerTest : IClassFixture<ProductServiceFixture>
{
    private readonly ProductServiceFixture _fixture;
    public ProductControllerTest(ProductServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Create_product_in_DataBase()
    {
        var product = new Filler<ProductModel>().Create();
        var categoryId = await _fixture.categoryService.AddCategory("Test");

        product.CategoryId = categoryId;
        var productController =
            new ProductController(_fixture.productService);

        //Act
        var result = await productController.CreateProduct(product) as CreatedResult;

        //Assert
        var lastProduct = await _fixture.dataBaseContext.Products.FindAsync(result.Value);
        Assert.NotNull(lastProduct);
        Assert.Equal(product.Name, lastProduct.Name);
        Assert.Equal(product.CategoryId, lastProduct.CategoryId);
        var returnOk = result as CreatedResult;
        Assert.NotNull(returnOk);
        Assert.IsType<Guid>(returnOk.Value);
    }
}