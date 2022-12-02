using System;
using System.Linq.Expressions;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Commands.Products;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Products;

[TestClass]
public class DeleteProductCommandTests : BaseCommandTests<Product, IProductWriterRepository>
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
        var upsertProduct = ProductFactory.CreateUpsert();

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Product, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                It.IsAny<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>()))
            .ReturnsAsync(Result.Ok(upsertProduct));

        WriterRepositoryMock.Setup(ls => ls.CheckIfIsUsedAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());


        WriterRepositoryMock.Setup(ls => ls.DeleteAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        //Act
        var result = await _deleteProductCommandHandler.Handle(new DeleteProductCommand
        {
            Id = upsertProduct.Id.GetValueOrDefault()
        }, CancellationToken.None);

        //Assert
        Assert.IsTrue(result.Success);
    }


    [TestMethod]
    public async Task GivenWrongProductId_WhenDeleteAsync_ThenShouldReturnError()
    {
        //Arrange
        var upsertProduct = ProductFactory.CreateUpsert();
        var error = Result.Error<Product>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Product, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                It.IsAny<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>()))
            .ReturnsAsync(error);

        WriterRepositoryMock.Setup(ls => ls.CheckIfIsUsedAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        WriterRepositoryMock.Setup(ls => ls.DeleteAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        //Act
        var result = await _deleteProductCommandHandler.Handle(new DeleteProductCommand
        {
            Id = upsertProduct.Id.GetValueOrDefault()
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
        var upsertProduct = ProductFactory.CreateUpsert();
        var error = Result.Error<Product>(HttpStatusCode.BadRequest, "[InUseNotDeleted]", ErrorMessages.InUseNotDeleted);

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Product, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                It.IsAny<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>()))
            .ReturnsAsync(Result.Ok(upsertProduct));

        WriterRepositoryMock.Setup(ls => ls.CheckIfIsUsedAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(error);


        WriterRepositoryMock.Setup(ls => ls.DeleteAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        //Act
        var result = await _deleteProductCommandHandler.Handle(new DeleteProductCommand
        {
            Id = upsertProduct.Id.GetValueOrDefault()
        }, CancellationToken.None);

        //Assert
        Assert.IsTrue(result.HasErrors);
        Assert.AreEqual(result.ErrorMessage, error.ErrorMessage);
        Assert.AreEqual(result.HttpStatusCode, error.HttpStatusCode);
    }
}