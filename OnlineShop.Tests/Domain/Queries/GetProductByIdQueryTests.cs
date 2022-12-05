using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Queries;

[TestClass]
public class GetProductByIdQueryTests: BaseQueryTests
{
    private List<Product> _products;
    private GetProductByIdQuery.GetProductByIdQueryHandler _getProductByIdQueryHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _products = ProductFactory.Create();
        _getProductByIdQueryHandler = new GetProductByIdQuery.GetProductByIdQueryHandler(ReaderRepositoryMock.Object);

    }

    [TestMethod]
    public async Task WhenGetProducts_ThenShouldReturnProducts()
    {
        //Arrange
        var query = new GetProductByIdQuery
        {
            Id = _products.First().Id.GetValueOrDefault()
        };
        ReaderRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Product, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                It.IsAny<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>()))
            .ReturnsAsync(Result.Ok(_products.First()));

        //Act
        var result = await _getProductByIdQueryHandler.Handle(query, CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<Product>.AreEntriesEqual(result.Data, _products.First()));
        Assert.AreEqual(query.Id, _products.First().Id.GetValueOrDefault());

    }
}