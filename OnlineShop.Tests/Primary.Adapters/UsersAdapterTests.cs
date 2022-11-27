using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Primary.Adapters.Implementation;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Users;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;
using pp = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Tests.Primary.Adapters
{
    [TestClass]
    public class UsersAdapterTests : BaseTests
    {
        private pp.LoginUser _loginUser;
        private pp.RegisterUser _registerUser;
        private UsersAdapter _usersAdapter;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            _usersAdapter = new UsersAdapter(MediatorMock.Object);
            _registerUser = UserFactory.CreateRegisterUser();
            _loginUser = UserFactory.CreateLoginUser();
        }

        [TestMethod]
        public async Task GiveUserDetails_WhenRegisterAsync_ThenReturnResultOk()
        {
            //Arrange
            MediatorMock.Setup(m => m.Send(It.IsAny<IRegisterCommand>(), CancellationToken.None))
                         .ReturnsAsync(Result.Ok());

            //Act
            var actualResult = await _usersAdapter.RegisterAsync(_registerUser, CancellationToken.None);

            //Assert
            Assert.IsTrue(actualResult.Success);
        }

        [TestMethod]
        public async Task GivenExistingUserDetails_WhenRegisterAsync_ThenReturnResultAlreadyExist()
        {
            //Arrange
            MediatorMock.Setup(m => m.Send(It.IsAny<IRegisterCommand>(), CancellationToken.None))
                         .ReturnsAsync(Result.Error<bool>(HttpStatusCode.Conflict, 
                                                          "[AlreadyExist]",
                                                          ErrorMessages.UserAlreadyExist));
            //Act
            var actualResult = await _usersAdapter.RegisterAsync(_registerUser, CancellationToken.None);

            //Assert
            Assert.IsTrue(ModelAssertionsUtils<pp.User>.IsCorrectError(HttpStatusCode.Conflict,
                                                                       "[AlreadyExist]",
                                                                       actualResult.HttpStatusCode,
                                                                       actualResult.ErrorMessage));
        }

        [TestMethod]
        public async Task GivenUsernameAndPassword_WhenLoginAsync_ThenReturnResultOk()
        {
            //Arrange

            const string token = "Token";
            MediatorMock.Setup(m => m.Send(It.IsAny<ILoginQuery>(), CancellationToken.None))
                         .ReturnsAsync(Result.Ok(token));

            //Act
            var actualResult = await _usersAdapter.LoginAsync(_loginUser, CancellationToken.None);

            //Assert
            Assert.IsTrue(actualResult.Success);
            Assert.AreEqual(actualResult.HttpStatusCode, HttpStatusCode.OK);
            Assert.AreEqual(actualResult.Data, "Token");
        }

        [TestMethod]
        public async Task GivenWrongUsernameAndCorrectPassword_WhenLoginAsync_ThenReturnResultNotFound()
        {
            //Arrange
            MediatorMock.Setup(m => m.Send(It.IsAny<ILoginQuery>(), CancellationToken.None))
                         .ReturnsAsync(Result.Error<string>(HttpStatusCode.NotFound, "[NotFound]",
                                                           ErrorMessages.NotFound));

            //Act
            var actualResult = await _usersAdapter.LoginAsync(_loginUser, CancellationToken.None);

            //Assert
            Assert.IsTrue(ModelAssertionsUtils<pp.User>.IsCorrectError(HttpStatusCode.NotFound,
                                                                       "[NotFound]",
                                                                       actualResult.HttpStatusCode,
                                                                       actualResult.ErrorMessage));
        }

        [TestMethod]
        public async Task GivenCorrectUsernameAndWrongPassword_WhenLoginAsync_ThenReturnResultNotFound()
        {
            //Arrange
            _loginUser.Password = Guid.NewGuid().ToString();
            MediatorMock.Setup(m => m.Send(It.IsAny<ILoginQuery>(), CancellationToken.None))
                         .ReturnsAsync(Result.Error<string>(HttpStatusCode.NotFound, "[NotFound]",
                                                           ErrorMessages.NotFound));

            //Act
            var actualResult = await _usersAdapter.LoginAsync(_loginUser, CancellationToken.None);

            //Assert
            Assert.IsTrue(ModelAssertionsUtils<pp.User>.IsCorrectError(HttpStatusCode.NotFound,
                                                                       "[NotFound]",
                                                                       actualResult.HttpStatusCode,
                                                                       actualResult.ErrorMessage));
        }
    }
}