using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Primary.Adapters.Implementation;
using OnlineShop.Primary.Adapters.Mappers;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;
using System.Threading.Tasks;
using System.Threading;
using Amazon.S3;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Products;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;

namespace OnlineShop.Tests.Primary.Adapters;

[TestClass]
public class ProductsAdapterTests : BaseTests
{
    private secondaryPorts.ProductDb _firstProductDb;
    private List<secondaryPorts.ProductDb> _products;
    private ProductsAdapter _productsAdapter;
    private Mock<IAmazonS3> _s3ClientMock;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _s3ClientMock = new Mock<IAmazonS3>();
        _products = ProductFactory.Create();
        _productsAdapter = new ProductsAdapter(MediatorMock.Object, _s3ClientMock.Object);
        _firstProductDb = _products.First().ToEntity();
    }

    [TestMethod]
    public async Task WhenGetAsyncProducts_ThenShouldReturnProducts()
    {
        //Arrange
        MediatorMock.Setup(m => m.Send(It.IsAny<IGetProductsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(_products));

        //Act
        var result = await _productsAdapter.GetAllAsync(CancellationToken.None);

        //Assert
        Assert.IsTrue(ModelAssertionsUtils<primaryPorts.ProductModel>.AreListsEqual(result.Data, _products.MapToPrimary()));
    }

    [TestMethod]
    public async Task WhenGetByIdAsync_ThenShouldReturnProduct()
    {
        //Arrange
        MediatorMock.Setup(m => m.Send(It.IsAny<IGetProductByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(_firstProductDb));

        //Act
        var result = await _productsAdapter.GetById(_firstProductDb.Id.GetValueOrDefault(), CancellationToken.None);

        //Assert
        Assert.IsTrue(ModelAssertionsUtils<primaryPorts.ProductModel>.AreEntriesEqual(result.Data, _firstProductDb.MapToPrimary()));
    }
    [TestMethod]
    public async Task GivenProducts_WhenInsertAsync_ThenShouldReturnIds()
    {
        //Arrange
        var locationIds = _products.Select(l => l.Id.GetValueOrDefault()).FirstOrDefault();

        MediatorMock.Setup(m => m.Send(It.IsAny<IAddProductsCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(locationIds));

        var upsertProduct = ProductFactory.CreateUpsertModels().FirstOrDefault();

        //Act
        var result = await _productsAdapter.InsertAsync(upsertProduct, CancellationToken.None);

        //Assert
        Assert.AreEqual(locationIds, result.Data);
    }

    [TestMethod]
    public async Task GivenProductAndId_WhenUpdateAsync_ThenShouldReturnId()
    {
        //Arrange
        var upsertCategory = ProductFactory.CreateUpsertModel();

        MediatorMock.Setup(ls => ls.Send(It.IsAny<IUpdateProductCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_firstProductDb.Id.GetValueOrDefault()));

        //Act
        var result = await _productsAdapter.UpdateAsync(_firstProductDb.Id.GetValueOrDefault(), upsertCategory, CancellationToken.None);

        //Assert
        Assert.AreEqual(_firstProductDb.Id.GetValueOrDefault(), result.Data);
    }

    [TestMethod]
    public async Task GivenProductId_WhenDeleteAsync_ThenShouldReturnOk()
    {
        //Arrange

        MediatorMock.Setup(ls => ls.Send(It.IsAny<IDeleteProductCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        //Act
        var result = await _productsAdapter.DeleteAsync(_firstProductDb.Id.GetValueOrDefault(),CancellationToken.None);

        //Assert
        Assert.IsTrue(result.Success);
    }
}