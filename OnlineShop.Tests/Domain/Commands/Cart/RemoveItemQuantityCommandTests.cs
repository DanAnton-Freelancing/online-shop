using System;
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

namespace OnlineShop.Tests.Domain.Commands.Cart;

[TestClass]
public class RemoveItemQuantityCommandTests : BaseCommandTests<CartItem, ICartItemWriterRepository>
{
    private RemoveItemFromCartCommand.RemoveItemFromCartCommandHandler _removeItemFromCartCommandHandler;
    private Mock<IProductWriterRepository> _productWriterRepositoryMock;
    private UserCart _userCart;
    private CartItem _cartItem;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _productWriterRepositoryMock = new Mock<IProductWriterRepository>(MockBehavior.Strict) { CallBase = true };

        _removeItemFromCartCommandHandler = new RemoveItemFromCartCommand.RemoveItemFromCartCommandHandler(WriterRepositoryMock.Object,
            _productWriterRepositoryMock.Object);
        _userCart = UserFactory.CreateUserCart();
        _cartItem = UserCartFactory.CreateCartItem(_userCart);
            
        _productWriterRepositoryMock.Setup(p => p.SaveAsync(It.IsAny<Product>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_cartItem.Product.Id.GetValueOrDefault()));

        WriterRepositoryMock.Setup(uc => uc.GetWithDetailsAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_cartItem));

        WriterRepositoryMock.Setup(uc => uc.SaveAndGetAsync(It.IsAny<CartItem>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_cartItem));

        WriterRepositoryMock.Setup(ci => ci.DeleteAsync(It.IsAny<Guid>(), CancellationToken.None))
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
        WriterRepositoryMock.Setup(uc => uc.GetWithDetailsAsync(It.IsAny<Guid>(), CancellationToken.None))
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