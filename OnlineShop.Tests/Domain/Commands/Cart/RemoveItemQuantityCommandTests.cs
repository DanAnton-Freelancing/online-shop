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
public class RemoveItemQuantityCommandTests : BaseCommandTests<CartItemDb>
{
    private RemoveItemFromCartCommand.RemoveItemFromCartCommandHandler _removeItemFromCartCommandHandler;
    private UserCartDb _userCartDb;
    private CartItemDb _cartItemDb;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();

        _removeItemFromCartCommandHandler = new RemoveItemFromCartCommand.RemoveItemFromCartCommandHandler(WriterRepositoryMock.Object);
        _userCartDb = UserFactory.CreateUserCart();
        _cartItemDb = UserCartFactory.CreateCartItem(_userCartDb);

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

        WriterRepositoryMock.Setup(ci => ci.DeleteAsync<CartItemDb>(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        WriterRepositoryMock.Setup(uc => uc.SaveAsync(CancellationToken.None))
            .ReturnsAsync(Result.Ok());

    }

    [TestMethod]
    public async Task GivenCartItemId_WhenDeleteItemAsync_ThenShouldReturnOk()
    {
        //Act
        var actualResult = await _removeItemFromCartCommandHandler.Handle(
            new RemoveItemFromCartCommand{CartItemId = _cartItemDb.Id.GetValueOrDefault()}, CancellationToken.None);

        //Assert
        Assert.IsTrue(actualResult.Success);
    }

    [TestMethod]
    public async Task GivenInvalidCartItemId_WhenDeleteItemAsync_ThenShouldReturnNotFound()
    {
        //Arrange
        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<CartItemDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<CartItemDb>, IOrderedQueryable<CartItemDb>>>(),
                It.IsAny<Func<IQueryable<CartItemDb>, IIncludableQueryable<CartItemDb, object>>>()))
                            .ReturnsAsync(Result.Error<CartItemDb>(HttpStatusCode.NotFound, "[NotFound]",
                                ErrorMessages.NotFound));

        //Act
        var actualResult = await _removeItemFromCartCommandHandler.Handle(
            new RemoveItemFromCartCommand { CartItemId = Guid.Empty }, CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<CartItemDb>.IsCorrectError(HttpStatusCode.NotFound,
            "[NotFound]",
            actualResult.HttpStatusCode,
            actualResult.ErrorMessage));
    }
}