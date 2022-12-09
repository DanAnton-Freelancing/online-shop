using System;
using OnlineShop.Domain.Aggregate;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;

public interface IGetUserCartQuery: IQuery<Result<UserCartEntity>>
{
    Guid UserId { get; set; }
}