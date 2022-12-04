using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Commands.Cart;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Cart;

[TestClass]
public class UpdateItemQuantityCommandTests : BaseCommandTests<CartItem>
{
    private UpdateItemQuantityCommand.UpdateItemQuantityCommandHandler _updateItemQuantityCommandHandler;
    private UserCart _userCart;
    private CartItem _cartItem;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();

        _updateItemQuantityCommandHandler = new UpdateItemQuantityCommand.UpdateItemQuantityCommandHandler(WriterRepositoryMock.Object);
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

        WriterRepositoryMock.Setup(uc => uc.AddAsync(It.IsAny<CartItem>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_cartItem));

        WriterRepositoryMock.Setup(uc => uc.SaveAsync(It.IsAny<CartItem>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_cartItem.Id.GetValueOrDefault()));

    }


    [TestMethod]
    public async Task GivenCartItemIdAndQuantity_WhenUpdateQuantityAsync_ThenShouldReturnOk()
    {
        //Arrange
        const int quantity = 12;

        //Act
        var actualResult = await _updateItemQuantityCommandHandler.Handle(
            new UpdateItemQuantityCommand
            {
                CartItemId = _cartItem.Id.GetValueOrDefault(),
                Quantity = quantity
            }, CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
    }

    [TestMethod]
    public async Task GivenCartItemIdAndTheSameQuantity_WhenUpdateQuantityAsync_ThenShouldReturnNotChanged()
    {
        //Arrange
        const int quantity = 12;
        _cartItem.Quantity = 12;

        //Act
        var actualResult = await _updateItemQuantityCommandHandler.Handle(
            new UpdateItemQuantityCommand
            {
                CartItemId = _cartItem.Id.GetValueOrDefault(),
                Quantity = quantity
            }, CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<CartItem>.IsCorrectError(HttpStatusCode.NotModified,
            "[NotChanged]",
            actualResult.HttpStatusCode,
            actualResult.ErrorMessage));
    }

    [TestMethod]
    public async Task GivenCartItemIdAndExceededProductQuantity_WhenUpdateQuantityAsync_ThenShouldReturnQuantityExceeded()
    {
        //Arrange
        const int quantity = 30;
        _cartItem.Quantity = 12;
        _cartItem.Product.AvailableQuantity -= (decimal?)_cartItem.Quantity;

        //Act
        var actualResult = await _updateItemQuantityCommandHandler.Handle(
            new UpdateItemQuantityCommand
            {
                CartItemId = _cartItem.Id.GetValueOrDefault(),
                Quantity = quantity
            }, CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<CartItem>.IsCorrectError(HttpStatusCode.BadRequest,
            "[QuantityExceeded]",
            actualResult.HttpStatusCode,
            actualResult.ErrorMessage));
    }


}