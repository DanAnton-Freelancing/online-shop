using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.Adapters;

public interface ICrudAdapter<T, TI>
    where T : BaseModel
    where TI : BaseUpsertModel
{
    Task<Result<List<T>>> GetAllAsync(CancellationToken cancellationToken);
    Task<Result<List<Guid>>> InsertAsync(List<TI> entities, CancellationToken cancellationToken);
    Task<Result<Guid>> UpdateAsync(Guid id,
        TI entity, CancellationToken cancellationToken);
    Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
}