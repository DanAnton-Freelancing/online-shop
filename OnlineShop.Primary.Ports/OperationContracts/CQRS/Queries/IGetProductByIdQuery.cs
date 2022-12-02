using System;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;

public interface IGetProductByIdQuery: IQuery<Result<Product>>
{
    public Guid Id { get; set; }
}