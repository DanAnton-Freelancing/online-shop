using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Queries
{
    [TestClass]
    public class GetUserCartQueryTests : BaseTests
    {
        private Mock<IUserCartReaderRepository> _userCartReaderRepositoryMock;
        private UserCart _userCart;

        private GetUserCartQuery.GetUserCartQueryHandler _getUserCartQueryHandler;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            _userCart = UserFactory.CreateUserCart();
            _userCartReaderRepositoryMock = new Mock<IUserCartReaderRepository>(MockBehavior.Strict) {CallBase = true};
            _getUserCartQueryHandler =
                new GetUserCartQuery.GetUserCartQueryHandler(_userCartReaderRepositoryMock.Object);

        }


        [TestMethod]
        public async Task GivenUserId_WhenGetWithDetailsAsync_ThenShouldReturnUserWithDetails()
        {
            //Arrange
            _userCartReaderRepositoryMock.Setup(uc => uc.GetWithDetailsAsync(It.IsAny<Guid>(), CancellationToken.None))
                                         .ReturnsAsync(Result.Ok(_userCart));

            //Act
            var actualResult = await _getUserCartQueryHandler.Handle(new GetUserCartQuery
                {
                    userId = _userCart.Id.GetValueOrDefault()
                },CancellationToken.None);

            //Assert
            Assert.IsTrue(EntitiesAssertionsUtils<UserCart>.AreEntriesEqual(_userCart, actualResult.Data));
        }
    }
}