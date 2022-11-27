using System;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Categories
{
    public interface IUpdateCategoryCommand : IUpdateCommand<Category, Result<Guid>>
    {
    }
}