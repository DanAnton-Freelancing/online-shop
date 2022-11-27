using System;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.Adapters
{
    public interface IUserCartAdapter
    {
        Task<Result<UserCart>> GetWithDetailsAsync(Guid userId, CancellationToken cancellationToken);
        
        Task<Result> AddItemAsync(UpsertCartItem item, Guid userId, CancellationToken cancellationToken);
        
        Task<Result> RemoveItemAsync(Guid itemId, CancellationToken cancellationToken);
        
        Task<Result> UpdateItemQuantityAsync(Guid itemId, double quantity,CancellationToken cancellationToken);
    }
}