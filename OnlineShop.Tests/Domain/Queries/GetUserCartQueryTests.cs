using System;
using System.Linq;
using System.Linq.Expressions;
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
public class GetUserCartQueryTests : BaseQueryTests
{
    private UserCartDb _userCartDb;

    private GetUserCartQuery.GetUserCartQueryHandler _getUserCartQueryHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _userCartDb = UserFactory.CreateUserCart();
        _getUserCartQueryHandler = new GetUserCartQuery.GetUserCartQueryHandler(ReaderRepositoryMock.Object);

    }

    [TestMethod]
    public async Task GivenUserId_WhenGetWithDetailsAsync_ThenShouldReturnUserWithDetails()
    {
        //Arrange
        var query = new GetUserCartQuery
        {
            UserId = _userCartDb.Id.GetValueOrDefault()
        };

        ReaderRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<UserCartDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<UserCartDb>, IOrderedQueryable<UserCartDb>>>(),
                It.IsAny<Func<IQueryable<UserCartDb>, IIncludableQueryable<UserCartDb, object>>>()))
            .ReturnsAsync(Result.Ok(_userCartDb));
        //Act
        var actualResult = await _getUserCartQueryHandler.Handle(query,CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<UserCartDb>.AreEntriesEqual(_userCartDb, actualResult.Data));
        Assert.IsNotNull(query.UserId);
    }
}