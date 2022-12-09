using System;
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
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Cart;

[TestClass]
public class UpdateItemQuantityCommandTests : BaseCommandTests<CartItemDb>
{
    private UpdateItemQuantityCommand.UpdateItemQuantityCommandHandler _updateItemQuantityCommandHandler;
    private UserCartDb _userCartDb;
    private CartItemDb _cartItemDb;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();

        _updateItemQuantityCommandHandler = new UpdateItemQuantityCommand.UpdateItemQuantityCommandHandler(WriterRepositoryMock.Object);
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

        WriterRepositoryMock.Setup(uc => uc.AddAsync(It.IsAny<CartItemDb>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_cartItemDb));

        WriterRepositoryMock.Setup(uc => uc.SaveAsync(It.IsAny<CartItemDb>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_cartItemDb.Id.GetValueOrDefault()));

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
                CartItemId = _cartItemDb.Id.GetValueOrDefault(),
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
        _cartItemDb.Quantity = 12;

        //Act
        var actualResult = await _updateItemQuantityCommandHandler.Handle(
            new UpdateItemQuantityCommand
            {
                CartItemId = _cartItemDb.Id.GetValueOrDefault(),
                Quantity = quantity
            }, CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<CartItemDb>.IsCorrectError(HttpStatusCode.NotModified,
            "[NotChanged]",
            actualResult.HttpStatusCode,
            actualResult.ErrorMessage));
    }

    [TestMethod]
    public async Task GivenCartItemIdAndExceededProductQuantity_WhenUpdateQuantityAsync_ThenShouldReturnQuantityExceeded()
    {
        //Arrange
        const int quantity = 30;
        _cartItemDb.Quantity = 12;
        _cartItemDb.ProductDb.AvailableQuantity -= (decimal?)_cartItemDb.Quantity;

        //Act
        var actualResult = await _updateItemQuantityCommandHandler.Handle(
            new UpdateItemQuantityCommand
            {
                CartItemId = _cartItemDb.Id.GetValueOrDefault(),
                Quantity = quantity
            }, CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<CartItemDb>.IsCorrectError(HttpStatusCode.BadRequest,
            "[QuantityExceeded]",
            actualResult.HttpStatusCode,
            actualResult.ErrorMessage));
    }


}