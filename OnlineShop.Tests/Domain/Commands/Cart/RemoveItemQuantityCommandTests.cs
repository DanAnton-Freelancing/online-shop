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
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Cart;

[TestClass]
public class RemoveItemQuantityCommandTests : BaseCommandTests<CartItem>
{
    private RemoveItemFromCartCommand.RemoveItemFromCartCommandHandler _removeItemFromCartCommandHandler;
    private UserCart _userCart;
    private CartItem _cartItem;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();

        _removeItemFromCartCommandHandler = new RemoveItemFromCartCommand.RemoveItemFromCartCommandHandler(WriterRepositoryMock.Object);
        _userCart = UserFactory.CreateUserCart();
        _cartItem = UserCartFactory.CreateCartItem(_userCart);

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

        WriterRepositoryMock.Setup(ci => ci.DeleteAsync<CartItem>(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        WriterRepositoryMock.Setup(uc => uc.SaveAsync(CancellationToken.None))
            .ReturnsAsync(Result.Ok());

    }

    [TestMethod]
    public async Task GivenCartItemId_WhenDeleteItemAsync_ThenShouldReturnOk()
    {
        //Act
        var actualResult = await _removeItemFromCartCommandHandler.Handle(
            new RemoveItemFromCartCommand{CartItemId = _cartItem.Id.GetValueOrDefault()}, CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
    }

    [TestMethod]
    public async Task GivenInvalidCartItemId_WhenDeleteItemAsync_ThenShouldReturnNotFound()
    {
        //Arrange
        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<CartItem, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<CartItem>, IOrderedQueryable<CartItem>>>(),
                It.IsAny<Func<IQueryable<CartItem>, IIncludableQueryable<CartItem, object>>>()))
                            .ReturnsAsync(Result.Error<CartItem>(HttpStatusCode.NotFound, "[NotFound]",
                                ErrorMessages.NotFound));

        //Act
        var actualResult = await _removeItemFromCartCommandHandler.Handle(
            new RemoveItemFromCartCommand { CartItemId = Guid.Empty }, CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<CartItem>.IsCorrectError(HttpStatusCode.NotFound,
            "[NotFound]",
            actualResult.HttpStatusCode,
            actualResult.ErrorMessage));
    }
}