using System;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.Adapters;

public interface IImagesAdapter
{
    Task<Result<Image>> Get(Guid imageId, CancellationToken cancellationToken);
}