using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Commands.Users;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Users;

[TestClass]
public class RegisterCommandTests : BaseCommandTests<User, IUserWriterRepository>
{
    private User _user;
    private RegisterCommand.RegisterCommandHandler _registerCommandHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _registerCommandHandler = new RegisterCommand.RegisterCommandHandler(WriterRepositoryMock.Object);
        _user = UserFactory.Create().ToEntity();
    }


    [TestMethod]
    public async Task GiveUserDetails_WhenRegisterAsync_ThenReturnResultOk()
    {
        //Arrange
        var userEntity = _user.ToEntity();
        WriterRepositoryMock.Setup(uw => uw.SaveAsync(It.IsAny<User>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(userEntity.Id.GetValueOrDefault()));

        WriterRepositoryMock.Setup(uw => uw.UserNotExistsAsync(It.IsAny<User>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());
        //Act
        var actualResult = await _registerCommandHandler.Handle(new RegisterCommand{Data = _user},CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
    }

    [TestMethod]
    public async Task GivenExistingUserDetails_WhenRegisterAsync_ThenReturnResultAlreadyExist()
    {
        //Arrange
        var userEntity = _user.ToEntity();
        WriterRepositoryMock.Setup(uw => uw.SaveAsync(It.IsAny<User>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(userEntity.Id.GetValueOrDefault()));

        WriterRepositoryMock.Setup(uw => uw.UserNotExistsAsync(It.IsAny<User>(), CancellationToken.None))
            .ReturnsAsync(Result.Error<bool>(HttpStatusCode.Conflict, 
                "[AlreadyExist]",
                ErrorMessages.UserAlreadyExist));
        //Act
        var actualResult = await _registerCommandHandler.Handle(new RegisterCommand { Data = _user }, CancellationToken.None);

        //Assert
        Assert.AreEqual(actualResult.HttpStatusCode, HttpStatusCode.Conflict);
        Assert.AreEqual(actualResult.ErrorMessage, "[AlreadyExist]");
    }
}