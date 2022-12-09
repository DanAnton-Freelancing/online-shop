using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Application.Implementations.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Queries;

[TestClass]
public class LoginQueryTests : BaseQueryTests
{
    private UserD _userDb;

    private LoginQuery.LoginQueryHandler _loginQueryHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();

        _userDb = UserFactory.Create();
        _loginQueryHandler = new LoginQuery.LoginQueryHandler(ReaderRepositoryMock.Object, UserFactory.GetSecret());

    }

    [TestMethod]
    public async Task GivenUsernameAndPassword_WhenLoginAsync_ThenReturnResultOk()
    {
        //Arrange
        var userEntity = _userDb.ToEntity();
        var userPassword = _userDb.Password;

        userEntity.AddSalt();
        userEntity.AddPasswordHash();

        var query = new LoginQuery { Username = _userDb.Username, Password = userPassword };

        ReaderRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<UserDb, bool>>>(), 
                CancellationToken.None,
                It.IsAny<Func<IQueryable<UserDb>, IOrderedQueryable<UserDb>>>(),
                It.IsAny<Func<IQueryable<UserDb>, IIncludableQueryable<UserDb, object>>>()))
            .ReturnsAsync(Result.Ok(userEntity));

        //Act
        var actualResult = await _loginQueryHandler.Handle(query, CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
        Assert.AreEqual(actualResult.HttpStatusCode, HttpStatusCode.OK);
        Assert.IsNotNull(query.Username);
        Assert.IsNotNull(actualResult.Data);
    }

    [TestMethod]
    public async Task GivenWrongUsernameAndCorrectPassword_WhenLoginAsync_ThenReturnResultNotFound()
    {
        //Arrange
        var userEntity = _userDb.ToEntity();
        var userPassword = _userDb.Password;

        userEntity.AddSalt();
        userEntity.AddPasswordHash();
        ReaderRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<UserDb, bool>>>(),
                CancellationToken.None,
                It.IsAny<Func<IQueryable<UserDb>, IOrderedQueryable<UserDb>>>(),
                It.IsAny<Func<IQueryable<UserDb>, IIncludableQueryable<UserDb, object>>>()))
            .ReturnsAsync(Result.Error<UserDb>(HttpStatusCode.NotFound, "[NotFound]",
                ErrorMessages.NotFound));

        //Act
        var actualResult = await _loginQueryHandler.Handle(new LoginQuery { Username = _userDb.Username, Password = userPassword },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<UserDb>.IsCorrectError(HttpStatusCode.NotFound,
            "[NotFound]",
            actualResult.HttpStatusCode,
            actualResult.ErrorMessage));
    }

    [TestMethod]
    public async Task GivenCorrectUsernameAndWrongPassword_WhenLoginAsync_ThenReturnResultNotFound()
    {
        //Arrange
        var userEntity = _userDb.ToEntity();
        var userPassword = Guid.NewGuid().ToString();

        userEntity.AddSalt();
        userEntity.AddPasswordHash();

        ReaderRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<UserDb, bool>>>(),
                CancellationToken.None,
                It.IsAny<Func<IQueryable<UserDb>, IOrderedQueryable<UserDb>>>(),
                It.IsAny<Func<IQueryable<UserDb>, IIncludableQueryable<UserDb, object>>>()))
            .ReturnsAsync(Result.Ok(userEntity));

        //Act
        var actualResult = await _loginQueryHandler.Handle(new LoginQuery { Username = _userDb.Username, Password = userPassword },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<UserDb>.IsCorrectError(HttpStatusCode.NotFound,
            "[NotFound]",
            actualResult.HttpStatusCode,
            actualResult.ErrorMessage));
    }
}