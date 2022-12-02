using System;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts;

public interface ICategoryWriterRepository : IBaseWriterRepository<Category>
{
    Task<Result> CheckIfIsUsedAsync(Guid id, CancellationToken cancellationToken);
}