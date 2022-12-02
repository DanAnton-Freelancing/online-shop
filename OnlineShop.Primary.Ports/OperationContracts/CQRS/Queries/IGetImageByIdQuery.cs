using System;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;

public interface IGetImageByIdQuery: IQuery<Result<Image>>
{
    public Guid ImageId { get; set; }
}