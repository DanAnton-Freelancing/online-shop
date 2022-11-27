using System;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Cart
{
    public interface IUpdateItemQuantityCommand : ICommand<Result>
    {
        Guid CartItemId { get; set; }

        double Quantity { get; set; }

    }
}