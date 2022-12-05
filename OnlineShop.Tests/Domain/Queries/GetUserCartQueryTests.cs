using System;
using System.Linq;
using System.Linq.Expressions;
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
public class GetUserCartQueryTests : BaseQueryTests
{
    private UserCart _userCart;

    private GetUserCartQuery.GetUserCartQueryHandler _getUserCartQueryHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _userCart = UserFactory.CreateUserCart();
        _getUserCartQueryHandler = new GetUserCartQuery.GetUserCartQueryHandler(ReaderRepositoryMock.Object);

    }

    [TestMethod]
    public async Task GivenUserId_WhenGetWithDetailsAsync_ThenShouldReturnUserWithDetails()
    {
        //Arrange
        var query = new GetUserCartQuery
        {
            userId = _userCart.Id.GetValueOrDefault()
        };

        ReaderRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<UserCart, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<UserCart>, IOrderedQueryable<UserCart>>>(),
                It.IsAny<Func<IQueryable<UserCart>, IIncludableQueryable<UserCart, object>>>()))
            .ReturnsAsync(Result.Ok(_userCart));
        //Act
        var actualResult = await _getUserCartQueryHandler.Handle(query,CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<UserCart>.AreEntriesEqual(_userCart, actualResult.Data));
        Assert.IsNotNull(query.userId);
    }
}