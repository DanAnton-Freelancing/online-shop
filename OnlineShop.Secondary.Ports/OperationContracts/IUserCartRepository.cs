using System;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts
{
    public interface IUserCartRepository : IBaseRepository<UserCart>
    {
        Task<Result<UserCart>> GetWithDetailsAsync(Guid userId, CancellationToken cancellationToken);
    }
}