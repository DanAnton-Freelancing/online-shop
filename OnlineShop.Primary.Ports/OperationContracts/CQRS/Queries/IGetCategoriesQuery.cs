using System.Collections.Generic;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries
{
    public interface IGetCategoriesQuery: IQuery<Result<List<Category>>>
    {
    }
}