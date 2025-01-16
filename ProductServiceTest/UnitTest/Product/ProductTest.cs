using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Controllers;
using ProductService.Services;
using ProductServiceTest.UnitTest.MockData;
using Tynamix.ObjectFiller;

namespace ProductServiceTest.UnitTest.Product;

public class ProductTest
{

    [Fact]
    public void Edit_Name()
    {
        var product = new ProductService.Domain.Entities.Product()
        {
            Id = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            Name = "Product2",
            Description = "..",
            Image = "a",
            Price = 12000,
        };

        var newName = "name";
        product.EditName(newName);

        Assert.Equal(newName, product.Name);
    }

    [Fact]
    public void EditName_ShouldThrowArgumentNullException_WhenNewNameIsNull()
    {
        var product = new Filler<ProductService.Domain.Entities.Product>().Create();

        string editedName = null;
        Assert.Throws<ArgumentNullException>(() => product.EditName(editedName));
    }



}



public class ProductControllerTest
{
    private ProductMockData _productMockData;
    public ProductControllerTest()
    {
        _productMockData = new ProductMockData();
    }

    [Fact]
    public async Task Get_All_Product()
    {
        var productServiceMoq = new Mock<IProductService>();
        productServiceMoq.Setup(c => c.GetAllProduct()).ReturnsAsync(await _productMockData.GetAllProducts());
        var productcontroller = new ProductController(productServiceMoq.Object);

        //Act
        var result = await productcontroller.AllProduct();

        //Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        var returnOk = result as OkObjectResult;
        Assert.NotNull(returnOk);
        Assert.IsType<List<ProductModel>>(returnOk.Value);
    }


    [Fact]
    public async Task Get_By_Id()
    {
        var id = Guid.NewGuid();
        var productServiceMoq = new Mock<IProductService>();
        productServiceMoq.Setup(c => c.GetProductBy(id)).ReturnsAsync(await _productMockData.GetByIdProducts(id));
        var productcontroller = new ProductController(productServiceMoq.Object);

        //Act
        var result = await productcontroller.GetProductBy(id);
        //Assert 
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        var returnOk = result as OkObjectResult;
        Assert.NotNull(returnOk);
        Assert.IsType<ProductModel>(returnOk.Value);
    }
}