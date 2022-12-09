using System;
using System.Collections.Generic;
using OnlineShop.Domain.Entities;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Categories;

public interface IAddCategoriesCommand : ICreateCommand<List<CategoryEntity>, Result<List<Guid>>>
{
}