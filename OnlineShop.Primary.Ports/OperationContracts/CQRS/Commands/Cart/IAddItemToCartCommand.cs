using System;
using OnlineShop.Domain.Aggregate;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Cart;

public interface IAddItemToCartCommand : ICommand<Result>
{
    CartItemEntity CartItemEntity { get; set; }
    Guid UserId { get; set; }
}