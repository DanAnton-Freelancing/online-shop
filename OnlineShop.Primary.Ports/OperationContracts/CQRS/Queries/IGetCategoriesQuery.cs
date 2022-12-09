using System.Collections.Generic;
using OnlineShop.Domain.Entities;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;

public interface IGetCategoriesQuery: IQuery<Result<List<CategoryEntity>>>
{
}