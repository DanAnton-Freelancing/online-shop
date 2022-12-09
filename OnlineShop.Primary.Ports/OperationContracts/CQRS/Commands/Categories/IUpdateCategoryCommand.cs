using System;
using OnlineShop.Domain.Entities;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Categories;

public interface IUpdateCategoryCommand : IUpdateCommand<CategoryEntity, Result<Guid>>
{
}