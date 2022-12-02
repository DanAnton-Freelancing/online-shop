using System.Collections.Generic;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;

public interface IGetProductsQuery: IQuery<Result<List<Product>>>
{
}