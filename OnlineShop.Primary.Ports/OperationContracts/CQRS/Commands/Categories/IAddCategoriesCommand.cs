using System;
using System.Collections.Generic;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Categories
{
    public interface IAddCategoriesCommand : ICreateCommand<List<Category>, Result<List<Guid>>>
    {
    }
}