using System;
using System.Collections.Generic;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Products
{
    public interface IAddProductsCommand : ICreateCommand<List<Product>, Result<List<Guid>>>
    {
    }
}