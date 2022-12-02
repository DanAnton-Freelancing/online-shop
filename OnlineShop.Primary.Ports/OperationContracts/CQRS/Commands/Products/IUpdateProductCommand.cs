using System;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Products;

public interface IUpdateProductCommand : IUpdateCommand<Product, Result<Guid>>
{
}