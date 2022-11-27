using System;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts
{
    public interface IProductWriterRepository : IBaseWriterRepository<Product>
    {
        Task<Result<Product>> GetWithChildAsync(Guid id, CancellationToken cancellationToken);
        Task<Result> CheckIfIsUsedAsync(Guid id, CancellationToken cancellationToken);

    }
}