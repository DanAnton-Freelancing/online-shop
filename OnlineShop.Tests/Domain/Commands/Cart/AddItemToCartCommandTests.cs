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
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Cart;

[TestClass]
public class AddItemToCartCommandTests : BaseCommandTests<UserCartDb>
{
    private AddItemToCartCommand.AddItemToCartCommandHandler _addItemToCartCommandHandler;
    private CartItemDb _cartItemDb;
    private UserCartDb _userCartDb;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();

        _addItemToCartCommandHandler = new AddItemToCartCommand.AddItemToCartCommandHandler(WriterRepositoryMock.Object);
        _userCartDb = UserFactory.CreateUserCart();
        _cartItemDb = UserCartFactory.CreateCartItem(_userCartDb);

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<ProductDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<ProductDb>, IOrderedQueryable<ProductDb>>>(),
                It.IsAny<Func<IQueryable<ProductDb>, IIncludableQueryable<ProductDb, object>>>()))
            .ReturnsAsync(Result.Ok(_cartItemDb.ProductDb));

        WriterRepositoryMock.Setup(p => p.AddAsync(It.IsAny<ProductDb>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_cartItemDb.ProductDb));

        WriterRepositoryMock.Setup(p => p.SaveAsync(It.IsAny<ProductDb>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_cartItemDb.ProductDb.Id.GetValueOrDefault()));

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<CartItemDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<CartItemDb>, IOrderedQueryable<CartItemDb>>>(),
                It.IsAny<Func<IQueryable<CartItemDb>, IIncludableQueryable<CartItemDb, object>>>()))
            .ReturnsAsync(Result.Ok(_cartItemDb));
        
        WriterRepositoryMock.Setup(uc => uc.AddAsync(It.IsAny<List<CartItemDb>>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(new List<CartItemDb> { _cartItemDb }));

        WriterRepositoryMock.Setup(uc => uc.SaveAsync(It.IsAny<List<CartItemDb>>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(new List<Guid> { _userCartDb.Id.GetValueOrDefault() }));

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<UserCartDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<UserCartDb>, IOrderedQueryable<UserCartDb>>>(),
                It.IsAny<Func<IQueryable<UserCartDb>, IIncludableQueryable<UserCartDb, object>>>()))
            .ReturnsAsync(Result.Ok(_userCartDb));

        WriterRepositoryMock.Setup(uc => uc.AddAsync(It.IsAny<UserCartDb>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_userCartDb));

        WriterRepositoryMock.Setup(uc => uc.SaveAsync(It.IsAny<UserCartDb>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_userCartDb.Id.GetValueOrDefault()));
    }

    [TestMethod]
    public async Task GivenCartItem_WhenAddItemAsync_ThenShouldReturnOk()
    {
        //Arrange
        _cartItemDb = UserCartFactory.CreateCartItem(_userCartDb);

        //Act
        var actualResult = await _addItemToCartCommandHandler.Handle(
            new AddItemToCartCommand { CartItem = _cartItemDb, UserId = _userCartDb.Id.GetValueOrDefault() },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
    }

    [TestMethod]
    public async Task GivenCartItemWithNoUserCart_WhenAddItemAsync_ThenShouldReturnOk()
    {
        //Arrange
        _userCartDb = null;

        //Act
        var actualResult = await _addItemToCartCommandHandler.Handle(
            new AddItemToCartCommand { CartItem = _cartItemDb, UserId = _userCartDb?.Id ?? Guid.Empty },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
    }

    [TestMethod]
    public async Task GivenCartItemWithExistingCartItems_WhenAddItemAsync_ThenShouldReturnOk()
    {
        //Arrange
        var cartItem1 = UserCartFactory.CreateCartItem(_userCartDb);
        _userCartDb.CartItems = new List<CartItemDb> { cartItem1 };

        var cartItem2 = UserCartFactory.CreateCartItem(_userCartDb);

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<ProductDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<ProductDb>, IOrderedQueryable<ProductDb>>>(),
                It.IsAny<Func<IQueryable<ProductDb>, IIncludableQueryable<ProductDb, object>>>()))
            .ReturnsAsync(Result.Ok(cartItem2.ProductDb));
        
        WriterRepositoryMock.Setup(p => p.AddAsync(It.IsAny<ProductDb>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(cartItem2.ProductDb));

        WriterRepositoryMock.Setup(p => p.SaveAsync(It.IsAny<ProductDb>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(cartItem2.ProductDb.Id.GetValueOrDefault()));

        //Act
        var actualResult = await _addItemToCartCommandHandler.Handle(
            new AddItemToCartCommand { CartItem = cartItem2, UserId = _userCartDb.Id.GetValueOrDefault() },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
    }

    [TestMethod]
    public async Task GivenCartItemWithExistingProductId_WhenAddItemAsync_ThenShouldReturnOk()
    {
        //Arrange
        _cartItemDb = UserCartFactory.CreateCartItem(_userCartDb);
        _userCartDb.CartItems = new List<CartItemDb> { _cartItemDb };

        //Act
        var actualResult = await _addItemToCartCommandHandler.Handle(
            new AddItemToCartCommand { CartItem = _cartItemDb, UserId = _userCartDb.Id.GetValueOrDefault() },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
    }

    [TestMethod]
    public async Task GivenEmptyCart_WhenAddItemAsync_ThenShouldReturnOk()
    {
        //Arrange
        _cartItemDb = UserCartFactory.CreateCartItem(_userCartDb);
        _userCartDb.CartItems = null;

        //Act
        var actualResult = await _addItemToCartCommandHandler.Handle(
            new AddItemToCartCommand { CartItem = _cartItemDb, UserId = _userCartDb.Id.GetValueOrDefault() },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
    }

    [TestMethod]
    public async Task GivenCartItemWithWrongProductId_WhenAddItemAsync_ThenShouldReturnError()
    {
        //Arrange
        _cartItemDb = UserCartFactory.CreateCartItem(_userCartDb);

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<ProductDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<ProductDb>, IOrderedQueryable<ProductDb>>>(),
                It.IsAny<Func<IQueryable<ProductDb>, IIncludableQueryable<ProductDb, object>>>()))
            .ReturnsAsync(Result.Error<ProductDb>(HttpStatusCode.NotFound, "[NotFound]",
                ErrorMessages.NotFound));

        //Act
        var actualResult = await _addItemToCartCommandHandler.Handle(
            new AddItemToCartCommand { CartItem = _cartItemDb, UserId = _userCartDb.Id.GetValueOrDefault() },
            CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<UserDb>.IsCorrectError(HttpStatusCode.NotFound,
            "[NotFound]",
            actualResult.HttpStatusCode,
            actualResult.ErrorMessage));
    }
}