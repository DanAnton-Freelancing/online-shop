using System;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Cart;

public interface IRemoveItemFromCartCommand : ICommand<Result>
{
    Guid CartItemId { get; set; }
}