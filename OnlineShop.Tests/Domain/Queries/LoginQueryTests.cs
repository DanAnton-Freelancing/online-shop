using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Queries;

[TestClass]
public class LoginQueryTests : BaseTests
{
    private User _user;

    private Mock<IUserReaderRepository> _userReaderRepositoryMock;
    private LoginQuery.LoginQueryHandler _loginQueryHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();

        _userReaderRepositoryMock = new Mock<IUserReaderRepository>(MockBehavior.Strict) { CallBase = true };
        _user = UserFactory.Create();
        _loginQueryHandler = new LoginQuery.LoginQueryHandler(_userReaderRepositoryMock.Object, UserFactory.GetSecret());

    }


    [TestMethod]
    public async Task GivenUsernameAndPassword_WhenLoginAsync_ThenReturnResultOk()
    {
        //Arrange
        var userEntity = _user.ToEntity();
        var userPassword = _user.Password;

        userEntity.AddSalt();
        userEntity.AddPasswordHash();

        _userReaderRepositoryMock.Setup(ur => ur.GetByUsernameAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(userEntity));

        //Act
        var actualResult = await _loginQueryHandler.Handle(new LoginQuery { Username = _user.Username, Password = userPassword },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
        Assert.AreEqual(actualResult.HttpStatusCode, HttpStatusCode.OK);
        Assert.IsNotNull(actualResult.Data);
    }

    [TestMethod]
    public async Task GivenWrongUsernameAndCorrectPassword_WhenLoginAsync_ThenReturnResultNotFound()
    {
        //Arrange
        var userEntity = _user.ToEntity();
        var userPassword = _user.Password;

        userEntity.AddSalt();
        userEntity.AddPasswordHash();

        _userReaderRepositoryMock.Setup(uw => uw.GetByUsernameAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(Result.Error<User>(HttpStatusCode.NotFound, "[NotFound]",
                ErrorMessages.NotFound));

        //Act
        var actualResult = await _loginQueryHandler.Handle(new LoginQuery { Username = _user.Username, Password = userPassword },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<User>.IsCorrectError(HttpStatusCode.NotFound,
            "[NotFound]",
            actualResult.HttpStatusCode,
            actualResult.ErrorMessage));
    }

    [TestMethod]
    public async Task GivenCorrectUsernameAndWrongPassword_WhenLoginAsync_ThenReturnResultNotFound()
    {
        //Arrange
        var userEntity = _user.ToEntity();
        var userPassword = Guid.NewGuid().ToString();

        userEntity.AddSalt();
        userEntity.AddPasswordHash();

        _userReaderRepositoryMock.Setup(uw => uw.GetByUsernameAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(userEntity));

        //Act
        var actualResult = await _loginQueryHandler.Handle(new LoginQuery { Username = _user.Username, Password = userPassword },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<User>.IsCorrectError(HttpStatusCode.NotFound,
            "[NotFound]",
            actualResult.HttpStatusCode,
            actualResult.ErrorMessage));
    }
}