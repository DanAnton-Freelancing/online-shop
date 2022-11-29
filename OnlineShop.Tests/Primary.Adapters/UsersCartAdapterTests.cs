using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Primary.Adapters.Implementation;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Cart;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Tests.Primary.Adapters
{
    [TestClass]
    public class UsersCartAdapterTests : BaseTests
    {
        private secondaryPorts.UserCart _userCart;
        private UserCartAdapter _userCartAdapter;
        private primaryPorts.UserCart _userCartModel;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            _userCartAdapter = new UserCartAdapter(MediatorMock.Object);
            _userCart = UserFactory.CreateUserCart();
            _userCartModel = UserFactory.CreateUserDetails();
            _userCart.ToEntity();
            _userCartModel.ToEntity(_userCart.Id.GetValueOrDefault());

            MediatorMock.Setup(m => m.Send(It.IsAny<IGetUserCartQuery>(), CancellationToken.None))
                        .ReturnsAsync(Result.Ok(_userCart));

            MediatorMock.Setup(m => m.Send(It.IsAny<IAddItemToCartCommand>(), CancellationToken.None))
                        .ReturnsAsync(Result.Ok());

            MediatorMock.Setup(m => m.Send(It.IsAny<IUpdateItemQuantityCommand>(),CancellationToken.None))
                        .ReturnsAsync(Result.Ok());

        }

        [TestMethod]
        public async Task GivenUserId_WhenGetWithDetailsAsync_ThenShouldReturnUserWithDetails()
        {
            //Act
            var actualResult = await _userCartAdapter.GetWithDetailsAsync(_userCart.Id.GetValueOrDefault(), CancellationToken.None);

            //Assert
            Assert.IsTrue(ModelAssertionsUtils<primaryPorts.UserCart>.AreEntriesEqual(_userCartModel,
                                                                            actualResult.Data));
        }

        [TestMethod]
        public async Task GivenEmptyUserId_WhenGetWithDetailsAsync_ThenShouldReturnError()
        {
            //Act
            var actualResult = await _userCartAdapter.GetWithDetailsAsync(Guid.Empty, CancellationToken.None);

            //Assert
            Assert.IsTrue(actualResult.HasErrors);
            Assert.AreEqual(actualResult.HttpStatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(actualResult.ErrorMessage, "[InvalidId]");
        }

        [TestMethod]
        public async Task GivenInvalidUserId_WhenGetWithDetails_ThenShouldReturnError()
        {
            //Arrange
            MediatorMock.Setup(m => m.Send(It.IsAny<IGetUserCartQuery>(),CancellationToken.None))
                        .ReturnsAsync(Result.Error<secondaryPorts.UserCart>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound));
            //Act
            var actualResult = await _userCartAdapter.GetWithDetailsAsync(Guid.NewGuid(), CancellationToken.None);

            //Assert
            Assert.IsTrue(ModelAssertionsUtils<primaryPorts.User>.IsCorrectError(HttpStatusCode.NotFound,
                                                                       "[NotFound]",
                                                                       actualResult.HttpStatusCode,
                                                                       actualResult.ErrorMessage));
        }

        [TestMethod]
        public async Task GivenCartItem_WhenAddItemAsync_ThenShouldReturnOk()
        {
            //Arrange
            var cartItem = UserCartFactory.CreateUpsertCartItem();
           
            //Act
            var actualResult = await _userCartAdapter.AddItemAsync(cartItem, _userCart.Id.GetValueOrDefault(), CancellationToken.None);

            //Assert
            Assert.IsTrue(actualResult.Success);
        }

        [TestMethod]
        public async Task GivenCartItemWithNoUserCart_WhenAddItemAsync_ThenShouldReturnOk()
        {
            //Arrange
            _userCart = null;
            var cartItem = UserCartFactory.CreateUpsertCartItem();

            //Act
            var actualResult = await _userCartAdapter.AddItemAsync(cartItem, _userCart?.Id ?? Guid.Empty, CancellationToken.None);

            //Assert
            Assert.IsTrue(actualResult.Success);
        }

        [TestMethod]
        public async Task GivenCartItemWithWrongProductId_WhenAddItemAsync_ThenShouldReturnError()
        {
            //Arrange
            var cartItem = UserCartFactory.CreateUpsertCartItem();
            cartItem.ProductId = Guid.Empty;

            //Act
            var actualResult = await _userCartAdapter.AddItemAsync(cartItem, _userCart.Id.GetValueOrDefault(), CancellationToken.None);

            //Assert
            Assert.IsTrue(ModelAssertionsUtils<primaryPorts.User>.IsCorrectError(HttpStatusCode.BadRequest,
                                                                       "[InvalidProductId]",
                                                                       actualResult.HttpStatusCode,
                                                                       actualResult.ErrorMessage));
        }

        [TestMethod]
        public async Task GivenCartItemWithWrongQuantity_WhenAddItemAsync_ThenShouldReturnError()
        {
            //Arrange
            var cartItem = UserCartFactory.CreateUpsertCartItem();
            cartItem.Quantity = -1;

            //Act
            var actualResult = await _userCartAdapter.AddItemAsync(cartItem, _userCart.Id.GetValueOrDefault(), CancellationToken.None);

            //Assert
            Assert.IsTrue(ModelAssertionsUtils<primaryPorts.User>.IsCorrectError(HttpStatusCode.BadRequest,
                                                                       "[InvalidQuantity]",
                                                                       actualResult.HttpStatusCode,
                                                                       actualResult.ErrorMessage));
        }

        [TestMethod]
        public async Task GivenInvalidCartItem_WhenAddItemAsync_ThenShouldReturnError()
        {
            //Act
            var actualResult = await _userCartAdapter.AddItemAsync(null, _userCart.Id.GetValueOrDefault(),CancellationToken.None);

            //Assert
            Assert.IsTrue(ModelAssertionsUtils<primaryPorts.User>.IsCorrectError(HttpStatusCode.BadRequest,
                                                                       "[InvalidInput]",
                                                                       actualResult.HttpStatusCode,
                                                                       actualResult.ErrorMessage));
        }

        [TestMethod]
        public async Task GivenCartItemId_WhenRemoveItemAsync_ThenShouldReturnOk()
        {
            //Arrange
            var cartItemId = Guid.NewGuid();

            MediatorMock.Setup(m => m.Send(It.IsAny<IRemoveItemFromCartCommand>(), CancellationToken.None))
                        .ReturnsAsync(Result.Ok());

            //Act
            var actualResult = await _userCartAdapter.RemoveItemAsync(cartItemId, CancellationToken.None);

            //Assert
            Assert.IsTrue(actualResult.Success);
        }

        [TestMethod]
        public async Task GivenInvalidCartItemId_WhenRemoveItemAsync_ThenShouldReturnNotFound()
        {
            //Arrange
            var cartItemId = Guid.NewGuid();

            MediatorMock.Setup(m => m.Send(It.IsAny<IRemoveItemFromCartCommand>(), CancellationToken.None))
                        .ReturnsAsync(Result.Error(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound));

            //Act
            var actualResult = await _userCartAdapter.RemoveItemAsync(cartItemId, CancellationToken.None);

            //Assert
            Assert.IsTrue(ModelAssertionsUtils<primaryPorts.CartItem>.IsCorrectError(HttpStatusCode.NotFound,
                                                                       "[NotFound]",
                                                                       actualResult.HttpStatusCode,
                                                                       actualResult.ErrorMessage));
        }

        [TestMethod]
        public async Task GivenCartItemIdAndQuantity_WhenUpdateQuantityAsync_ThenShouldReturnOk()
        {
            //Arrange
            var cartItemId = Guid.NewGuid();
            const int quantity = 12;

            //Act
            var actualResult = await _userCartAdapter.UpdateItemQuantityAsync(cartItemId, quantity, CancellationToken.None);

            //Assert
            Assert.IsTrue(actualResult.Success);
        }

        [TestMethod]
        public async Task GivenCartItemIdAndInvalidQuantity_WhenUpdateQuantityAsync_ThenShouldReturnInvalidQuantity()
        {
            //Arrange
            var cartItemId = Guid.NewGuid();
            const int quantity = -12;

            //Act
            var actualResult = await _userCartAdapter.UpdateItemQuantityAsync(cartItemId, quantity, CancellationToken.None);

            //Assert
            Assert.IsTrue(ModelAssertionsUtils<primaryPorts.CartItem>.IsCorrectError(HttpStatusCode.BadRequest,
                                                                           "[InvalidQuantity]",
                                                                           actualResult.HttpStatusCode,
                                                                           actualResult.ErrorMessage));
        }
    }
}