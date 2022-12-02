using System;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;

public interface IGetUserCartQuery: IQuery<Result<UserCart>>
{
    Guid userId { get; set; }
}