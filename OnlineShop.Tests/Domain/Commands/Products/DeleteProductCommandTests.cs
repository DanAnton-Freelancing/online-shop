using System;
using System.Linq.Expressions;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Application.Implementations.Commands.Products;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Products;

[TestClass]
public class DeleteProductCommandTests : BaseCommandTests<ProductDb>
{
    private DeleteProductCommand.DeleteProductCommandHandler _deleteProductCommandHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _deleteProductCommandHandler =
            new DeleteProductCommand.DeleteProductCommandHandler(WriterRepositoryMock.Object);
        Entities = ProductFactory.Create();
    }

    [TestMethod]
    public async Task GivenProductId_WhenDeleteAsync_ThenShouldReturnOk()
    {
        //Arrange
        var product = ProductFactory.CreateUpsert().ToEntity();

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<ProductDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<ProductDb>, IOrderedQueryable<ProductDb>>>(),
                It.IsAny<Func<IQueryable<ProductDb>, IIncludableQueryable<ProductDb, object>>>()))
            .ReturnsAsync(Result.Ok(product));

        WriterRepositoryMock.Setup(ls => ls.DeleteAsync<ProductDb>(product.Id.GetValueOrDefault(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        WriterRepositoryMock.Setup(ls => ls.SaveAsync(CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        //Act
        var result = await _deleteProductCommandHandler.Handle(new DeleteProductCommand
        {
            Id = product.Id.GetValueOrDefault()
        }, CancellationToken.None);

        //Assert
        Assert.IsTrue(result.Success);
    }


    [TestMethod]
    public async Task GivenWrongProductId_WhenDeleteAsync_ThenShouldReturnError()
    {
        //Arrange
        var product = ProductFactory.CreateUpsert().ToEntity();
        var error = Result.Error<ProductDb>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<ProductDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<ProductDb>, IOrderedQueryable<ProductDb>>>(),
                It.IsAny<Func<IQueryable<ProductDb>, IIncludableQueryable<ProductDb, object>>>()))
            .ReturnsAsync(error);

        WriterRepositoryMock.Setup(ls => ls.DeleteAsync<ProductDb>(product.Id.GetValueOrDefault(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        //Act
        var result = await _deleteProductCommandHandler.Handle(new DeleteProductCommand
        {
            Id = product.Id.GetValueOrDefault()
        }, CancellationToken.None);

        //Assert
        Assert.IsTrue(result.HasErrors);
        Assert.AreEqual(result.ErrorMessage, error.ErrorMessage);
        Assert.AreEqual(result.HttpStatusCode, error.HttpStatusCode);
    }


    [TestMethod]
    public async Task GivenInUseProductId_WhenDeleteAsync_ThenShouldReturnError()
    {
        //Arrange
        var product = ProductFactory.CreateUpsert().ToEntity();
        product.CartItemDb = new CartItemDb();
        var error = Result.Error<ProductDb>(HttpStatusCode.BadRequest, "[InUseNotDeleted]", ErrorMessages.InUseNotDeleted);

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<ProductDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<ProductDb>, IOrderedQueryable<ProductDb>>>(),
                It.IsAny<Func<IQueryable<ProductDb>, IIncludableQueryable<ProductDb, object>>>()))
            .ReturnsAsync(Result.Ok(product));


        WriterRepositoryMock.Setup(ls => ls.DeleteAsync<ProductDb>(product.Id.GetValueOrDefault(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        //Act
        var result = await _deleteProductCommandHandler.Handle(new DeleteProductCommand
        {
            Id = product.Id.GetValueOrDefault()
        }, CancellationToken.None);

        //Assert
        Assert.IsTrue(result.HasErrors);
        Assert.AreEqual(result.ErrorMessage, error.ErrorMessage);
        Assert.AreEqual(result.HttpStatusCode, error.HttpStatusCode);
    }
}