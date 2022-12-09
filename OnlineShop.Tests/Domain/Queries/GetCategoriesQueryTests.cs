using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Application.Implementations.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Queries;

[TestClass]
public class GetCategoriesQueryTests: BaseQueryTests
{
    private List<CategoryDb> _categories;
    private GetCategoriesQuery.GetCategoriesQueryHandler _getCategoriesQueryHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _categories = CategoryFactory.Create();
        _getCategoriesQueryHandler = new GetCategoriesQuery.GetCategoriesQueryHandler(ReaderRepositoryMock.Object);

    }

    [TestMethod]
    public async Task WhenGetProducts_ThenShouldReturnProducts()
    {
        //Arrange
        ReaderRepositoryMock.Setup(uc => uc.GetAsync(CancellationToken.None, It.IsAny<Expression<Func<CategoryDb, bool>>>(),
                It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
                It.IsAny<Func<IQueryable<CategoryDb>, IIncludableQueryable<CategoryDb, object>>>()))
            .ReturnsAsync(Result.Ok(_categories));

        //Act
        var result = await _getCategoriesQueryHandler.Handle(new GetCategoriesQuery(), CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<CategoryDb>.AreListsEqual(result.Data, _categories));

    }
}