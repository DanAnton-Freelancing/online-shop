using System;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Cart
{
    public interface IAddItemToCartCommand : ICommand<Result>
    {
        CartItem CartItem { get; set; }
        Guid UserId { get; set; }
    }
}