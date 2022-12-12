using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Application.Implementations.Commands.Products;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Products;

[TestClass]
public class UpdateProductCommandTests : BaseCommandTests<Product>
{

    private Product _firstProduct;
    private UpdateProductCommand.UpdateProductCommandHandler _updateProductsCommandHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _updateProductsCommandHandler = new UpdateProductCommand.UpdateProductCommandHandler(WriterRepositoryMock.Object);
        Entities = ProductFactory.Create();
        _firstProduct = Entities.First().ToEntity();
    }

    [TestMethod]
    public async Task GivenProductAndId_WhenUpdateAsync_ThenShouldReturnId()
    {
        //Arrange
            
        WriterRepositoryMock.Setup(ls => ls.AddAsync(It.IsAny<Product>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_firstProduct));

        WriterRepositoryMock.Setup(ls => ls.SaveAsync(It.IsAny<Product>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_firstProduct.Id.GetValueOrDefault()));

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Product, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                It.IsAny<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>()))
            .ReturnsAsync(Result.Ok(_firstProduct));
        
        //Act
        var result = await _updateProductsCommandHandler.Handle(new UpdateProductCommand
        {
            Data = _firstProduct,
            Id = _firstProduct.Id.GetValueOrDefault()
        }, CancellationToken.None);

        //Assert
        Assert.AreEqual(_firstProduct.Id, result.Data);
    }
}