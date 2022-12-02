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
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Queries;

[TestClass]
public class GetCategoriesQueryTests: BaseTests
{
    private Mock<ICategoryReaderRepository> _categoryReaderRepositoryMock;
    private List<Category> _categories;
    private GetCategoriesQuery.GetCategoriesQueryHandler _getCategoriesQueryHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _categoryReaderRepositoryMock = new Mock<ICategoryReaderRepository>(MockBehavior.Strict) { CallBase = true };
        _categories = CategoryFactory.Create();
        _getCategoriesQueryHandler = new GetCategoriesQuery.GetCategoriesQueryHandler(_categoryReaderRepositoryMock.Object);

    }

    [TestMethod]
    public async Task WhenGetProducts_ThenShouldReturnProducts()
    {
        //Arrange
        _categoryReaderRepositoryMock.Setup(uc => uc.GetAsync(CancellationToken.None, It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<Func<IQueryable<Category>, IIncludableQueryable<Category, object>>>()))
            .ReturnsAsync(Result.Ok(_categories));

        //Act
        var result = await _getCategoriesQueryHandler.Handle(new GetCategoriesQuery(), CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<Category>.AreListsEqual(result.Data, _categories));

    }
}