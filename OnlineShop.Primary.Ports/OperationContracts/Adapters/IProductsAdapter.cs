using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace OnlineShop.Primary.Ports.OperationContracts.Adapters;

public interface IProductsAdapter : ICrudAdapter<ProductModel, UpsertProductModel>
{
    Task<Result<ProductModel>> GetById(Guid id, CancellationToken cancellationToken);
    Task<Result<Guid>> InsertAsync(UpsertProductModel entity, CancellationToken cancellationToken);
}