using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Application.Implementations.Commands.Users;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.Mappers;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Users;

[TestClass]
public class RegisterCommandTests : BaseCommandTests<User>
{
    private User _user;
    private RegisterCommand.RegisterCommandHandler _registerCommandHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _registerCommandHandler = new RegisterCommand.RegisterCommandHandler(WriterRepositoryMock.Object);
        _user = UserFactory.Create().ToEntity();

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<User, bool>>>(),
                CancellationToken.None,
                null,
                null))
            .ReturnsAsync(Result.Ok(_user));

    }


    [TestMethod]
    public async Task GiveUserDetails_WhenRegisterAsync_ThenReturnResultOk()
    {
        //Arrange
        var userEntity = _user.ToEntity();

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<User, bool>>>(),
                CancellationToken.None,
                null,
                null))
            .ReturnsAsync(Result.Error<User>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound));

        WriterRepositoryMock.Setup(uw => uw.AddAsync(It.IsAny<User>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(userEntity));

        WriterRepositoryMock.Setup(uw => uw.SaveAsync(It.IsAny<User>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(userEntity.Id.GetValueOrDefault()));
        //Act
        var actualResult = await _registerCommandHandler.Handle(new RegisterCommand{Data = _user.MapToDomain()},CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
    }

    [TestMethod]
    public async Task GivenExistingUserDetails_WhenRegisterAsync_ThenReturnResultAlreadyExist()
    {
        //Arrange
        var userEntity = _user.ToEntity();

        WriterRepositoryMock.Setup(uw => uw.AddAsync(It.IsAny<User>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(userEntity));

        WriterRepositoryMock.Setup(uw => uw.SaveAsync(It.IsAny<User>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(userEntity.Id.GetValueOrDefault()));
        //Act
        var actualResult = await _registerCommandHandler.Handle(new RegisterCommand { Data = _user.MapToDomain() }, CancellationToken.None);

        //Assert
        Assert.AreEqual(actualResult.HttpStatusCode, HttpStatusCode.Conflict);
        Assert.AreEqual(actualResult.ErrorMessage, "[AlreadyExist]");
    }
}