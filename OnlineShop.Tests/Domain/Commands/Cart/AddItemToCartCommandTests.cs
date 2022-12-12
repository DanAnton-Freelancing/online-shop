using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Application.Implementations.Commands.Cart;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.Mappers;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Cart;

[TestClass]
public class AddItemToCartCommandTests : BaseCommandTests<UserCart>
{
    private AddItemToCartCommand.AddItemToCartCommandHandler _addItemToCartCommandHandler;
    private CartItem _cartItem;
    private UserCart _userCart;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();

        _addItemToCartCommandHandler = new AddItemToCartCommand.AddItemToCartCommandHandler(WriterRepositoryMock.Object);
        _userCart = UserFactory.CreateUserCart();
        _cartItem = UserCartFactory.CreateCartItem(_userCart);

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Product, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                It.IsAny<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>()))
            .ReturnsAsync(Result.Ok(_cartItem.Product));

        WriterRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Product>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_cartItem.Product));

        WriterRepositoryMock.Setup(p => p.SaveAsync(It.IsAny<Product>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_cartItem.Product.Id.GetValueOrDefault()));

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<CartItem, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<CartItem>, IOrderedQueryable<CartItem>>>(),
                It.IsAny<Func<IQueryable<CartItem>, IIncludableQueryable<CartItem, object>>>()))
            .ReturnsAsync(Result.Ok(_cartItem));
        
        WriterRepositoryMock.Setup(uc => uc.AddAsync(It.IsAny<List<CartItem>>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(new List<CartItem> { _cartItem }));

        WriterRepositoryMock.Setup(uc => uc.SaveAsync(It.IsAny<List<CartItem>>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(new List<Guid> { _userCart.Id.GetValueOrDefault() }));

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<UserCart, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<UserCart>, IOrderedQueryable<UserCart>>>(),
                It.IsAny<Func<IQueryable<UserCart>, IIncludableQueryable<UserCart, object>>>()))
            .ReturnsAsync(Result.Ok(_userCart));

        WriterRepositoryMock.Setup(uc => uc.AddAsync(It.IsAny<UserCart>(), CancellationToken.None))
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
        var actualResult = await _addItemToCartCommandHandler.Handle(
            new AddItemToCartCommand { CartItemEntity = _cartItem.MapToDomain(), UserId = _userCart.Id.GetValueOrDefault() },
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
        var actualResult = await _addItemToCartCommandHandler.Handle(
            new AddItemToCartCommand { CartItemEntity = _cartItem.MapToDomain(), UserId = _userCart?.Id ?? Guid.Empty },
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

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Product, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                It.IsAny<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>()))
            .ReturnsAsync(Result.Ok(cartItem2.Product));
        
        WriterRepositoryMock.Setup(p => p.AddAsync(It.IsAny<Product>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(cartItem2.Product));

        WriterRepositoryMock.Setup(p => p.SaveAsync(It.IsAny<Product>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(cartItem2.Product.Id.GetValueOrDefault()));

        //Act
        var actualResult = await _addItemToCartCommandHandler.Handle(
            new AddItemToCartCommand { CartItemEntity = cartItem2.MapToDomain(), UserId = _userCart.Id.GetValueOrDefault() },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
    }

    [TestMethod]
    public async Task GivenCartItemWithExistingProductId_WhenAddItemAsync_ThenShouldReturnOk()
    {
        //Arrange
        _cartItem = UserCartFactory.CreateCartItem(_userCart);
        _userCart.CartItems = new List<CartItem> { _cartItem };

        //Act
        var actualResult = await _addItemToCartCommandHandler.Handle(
            new AddItemToCartCommand { CartItemEntity = _cartItem.MapToDomain(), UserId = _userCart.Id.GetValueOrDefault() },
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
        var actualResult = await _addItemToCartCommandHandler.Handle(
            new AddItemToCartCommand { CartItemEntity = _cartItem.MapToDomain(), UserId = _userCart.Id.GetValueOrDefault() },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
    }

    [TestMethod]
    public async Task GivenCartItemWithWrongProductId_WhenAddItemAsync_ThenShouldReturnError()
    {
        //Arrange
        _cartItem = UserCartFactory.CreateCartItem(_userCart);

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Product, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                It.IsAny<Func<IQueryable<Product>, IIncludableQueryable<Product, object>>>()))
            .ReturnsAsync(Result.Error<Product>(HttpStatusCode.NotFound, "[NotFound]",
                ErrorMessages.NotFound));

        //Act
        var actualResult = await _addItemToCartCommandHandler.Handle(
            new AddItemToCartCommand { CartItemEntity = _cartItem.MapToDomain(), UserId = _userCart.Id.GetValueOrDefault() },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<User>.IsCorrectError(HttpStatusCode.NotFound,
            "[NotFound]",
            actualResult.HttpStatusCode,
            actualResult.ErrorMessage));
    }
}