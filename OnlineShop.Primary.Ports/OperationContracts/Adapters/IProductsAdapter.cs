using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace OnlineShop.Primary.Ports.OperationContracts.Adapters;

public interface IProductsAdapter : ICrudAdapter<Product, UpsertProduct>
{
    Task<Result<Product>> GetById(Guid id, CancellationToken cancellationToken);
    Task<Result<Guid>> InsertAsync(UpsertProduct entity, CancellationToken cancellationToken);
}