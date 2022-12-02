using System;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts;

public interface ICartItemWriterRepository : IBaseWriterRepository<CartItem>
{
    Task<Result<CartItem>> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken);
}