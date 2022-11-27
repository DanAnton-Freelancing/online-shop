using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Commands.Cart;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Cart
{

    [TestClass]
    public class AddItemToCartCommandTests : BaseCommandTests<UserCart, IUserCartWriterRepository>
    {
        private AddItemToCartCommand.AddItemToCartCommandHandler _addItemToCartCommandHandler;
        private Mock<IProductWriterRepository> _productWriterRepositoryMock;
        private Mock<IUserCartRepository> _userCartRepositoryMock;
        private Mock<ICartItemWriterRepository> _cartItemWriterRepositoryMock;
        private UserCart _userCart;
        private CartItem _cartItem;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            _productWriterRepositoryMock = new Mock<IProductWriterRepository>(MockBehavior.Strict) { CallBase = true };
            _cartItemWriterRepositoryMock = new Mock<ICartItemWriterRepository>(MockBehavior.Strict) { CallBase = true };
            _userCartRepositoryMock = WriterRepositoryMock.As<IUserCartRepository>();

            _addItemToCartCommandHandler = new AddItemToCartCommand.AddItemToCartCommandHandler(WriterRepositoryMock.Object,
                                                                                                _cartItemWriterRepositoryMock.Object,
                                                                                                _productWriterRepositoryMock.Object);
            _userCart = UserFactory.CreateUserCart();
            _cartItem = UserCartFactory.CreateCartItem(_userCart);
            
            _productWriterRepositoryMock.Setup(uc => uc.GetAsync(It.IsAny<Guid>(), CancellationToken.None))
                                        .ReturnsAsync(Result.Ok(_cartItem.Product));

            _productWriterRepositoryMock.Setup(p => p.SaveAsync(It.IsAny<Product>(), CancellationToken.None))
                                        .ReturnsAsync(Result.Ok(_cartItem.Product.Id.GetValueOrDefault()));

            _cartItemWriterRepositoryMock.Setup(uc => uc.GetWithDetailsAsync(It.IsAny<Guid>(), CancellationToken.None))
                                         .ReturnsAsync(Result.Ok(_cartItem));
            
            _cartItemWriterRepositoryMock.Setup(uc => uc.SaveAsync(It.IsAny<List<CartItem>>(),CancellationToken.None))
                                         .ReturnsAsync(Result.Ok(new List<Guid> { _userCart.Id.GetValueOrDefault() }));

            _userCartRepositoryMock.Setup(uc => uc.GetWithDetailsAsync(It.IsAny<Guid>(), CancellationToken.None))
                                   .ReturnsAsync(Result.Ok(_userCart));

            WriterRepositoryMock.Setup(uc => uc.SaveAsync(It.IsAny<UserCart>(), CancellationToken.None))
                                .ReturnsAsync(Result.Ok(_userCart.Id.GetValueOrDefault()));
            
        }

        [TestMethod]
        public async Task GivenCartItem_WhenAddItemAsync_ThenShouldReturnOk()
        {
            //Arrange
            _cartItem = UserCartFactory.CreateCartItem(_userCart);

            //Act
            var actualResult = await _addItemToCartCommandHandler.Handle(new AddItemToCartCommand { CartItem = _cartItem, UserId = _userCart.Id.GetValueOrDefault() },
                                                                         CancellationToken.None);

            //Assert
            Assert.IsTrue(actualResult.Success);
        }

        [TestMethod]
        public async Task GivenCartItemWithNoUserCart_WhenAddItemAsync_ThenShouldReturnOk()
        {
            //Arrange
            _userCart = null;

            //Act
            var actualResult = await _addItemToCartCommandHandler.Handle(new AddItemToCartCommand { CartItem = _cartItem, UserId = _userCart?.Id ?? Guid.Empty },
                                                                         CancellationToken.None);

            //Assert
            Assert.IsTrue(actualResult.Success);
        }

        [TestMethod]
        public async Task GivenCartItemWithExistingCartItems_WhenAddItemAsync_ThenShouldReturnOk()
        {
            //Arrange
            var cartItem1 = UserCartFactory.CreateCartItem(_userCart);
            _userCart.CartItems = new List<CartItem> { cartItem1 };

            var cartItem2 = UserCartFactory.CreateCartItem(_userCart);

            _productWriterRepositoryMock.Setup(uc => uc.GetAsync(It.IsAny<Guid>(), CancellationToken.None))
                                        .ReturnsAsync(Result.Ok(cartItem2.Product));

            _productWriterRepositoryMock.Setup(p => p.SaveAsync(It.IsAny<Product>(), CancellationToken.None))
                                        .ReturnsAsync(Result.Ok(cartItem2.Product.Id.GetValueOrDefault()));
            //Act
            var actualResult = await _addItemToCartCommandHandler.Handle(new AddItemToCartCommand { CartItem = cartItem2, UserId = _userCart.Id.GetValueOrDefault()},
                                                                         CancellationToken.None);

            //Assert
            Assert.IsTrue(actualResult.Success);
        }

        [TestMethod]
        public async Task GivenCartItemWithExistingProductId_WhenAddItemAsync_ThenShouldReturnOk()
        {
            //Arrange
            _cartItem  = UserCartFactory.CreateCartItem(_userCart);
            _userCart.CartItems = new List<CartItem> { _cartItem };

            //Act
            var actualResult = await _addItemToCartCommandHandler.Handle(new AddItemToCartCommand { CartItem = _cartItem, UserId = _userCart.Id.GetValueOrDefault() },
                                                                         CancellationToken.None);

            //Assert
            Assert.IsTrue(actualResult.Success);
        }

        [TestMethod]
        public async Task GivenEmptyCart_WhenAddItemAsync_ThenShouldReturnOk()
        {
            //Arrange
            _cartItem = UserCartFactory.CreateCartItem(_userCart);
            _userCart.CartItems = null;

            //Act
            var actualResult = await _addItemToCartCommandHandler.Handle(new AddItemToCartCommand { CartItem = _cartItem, UserId = _userCart.Id.GetValueOrDefault() },
                CancellationToken.None);

            //Assert
            Assert.IsTrue(actualResult.Success);
        }

        [TestMethod]
        public async Task GivenCartItemWithWrongProductId_WhenAddItemAsync_ThenShouldReturnError()
        {
            //Arrange
            _cartItem = UserCartFactory.CreateCartItem(_userCart); 

            _productWriterRepositoryMock.Setup(uc => uc.GetAsync(It.IsAny<Guid>(), CancellationToken.None))
                                        .ReturnsAsync(Result.Error<Product>(HttpStatusCode.NotFound, "[NotFound]",
                                            ErrorMessages.NotFound));

            //Act
            var actualResult = await _addItemToCartCommandHandler.Handle(new AddItemToCartCommand { CartItem = _cartItem, UserId = _userCart.Id.GetValueOrDefault() },
                                                                         CancellationToken.None);

            //Assert
            Assert.IsTrue(EntitiesAssertionsUtils<User>.IsCorrectError(HttpStatusCode.NotFound,
                                                                       "[NotFound]",
                                                                       actualResult.HttpStatusCode,
                                                                       actualResult.ErrorMessage));
        }

    }
}