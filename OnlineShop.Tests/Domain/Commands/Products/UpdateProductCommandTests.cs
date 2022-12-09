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
public class UpdateProductCommandTests : BaseCommandTests<ProductDb>
{

    private ProductDb _firstProductDb;
    private UpdateProductCommand.UpdateProductCommandHandler _updateProductsCommandHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _updateProductsCommandHandler = new UpdateProductCommand.UpdateProductCommandHandler(WriterRepositoryMock.Object);
        Entities = ProductFactory.Create();
        _firstProductDb = Entities.First().ToEntity();
    }

    [TestMethod]
    public async Task GivenProductAndId_WhenUpdateAsync_ThenShouldReturnId()
    {
        //Arrange
            
        WriterRepositoryMock.Setup(ls => ls.AddAsync(It.IsAny<ProductDb>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_firstProductDb));

        WriterRepositoryMock.Setup(ls => ls.SaveAsync(It.IsAny<ProductDb>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_firstProductDb.Id.GetValueOrDefault()));

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<ProductDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<ProductDb>, IOrderedQueryable<ProductDb>>>(),
                It.IsAny<Func<IQueryable<ProductDb>, IIncludableQueryable<ProductDb, object>>>()))
            .ReturnsAsync(Result.Ok(_firstProductDb));
        
        //Act
        var result = await _updateProductsCommandHandler.Handle(new UpdateProductCommand
        {
            Data = _firstProductDb,
            Id = _firstProductDb.Id.GetValueOrDefault()
        }, CancellationToken.None);

        //Assert
        Assert.AreEqual(_firstProductDb.Id, result.Data);
    }
}