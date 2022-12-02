using System;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Products
{
    public interface IAddProductsCommand : ICreateCommand<Product, Result<Guid>>
    {
    }
}