using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Primary.Ports.OperationContracts.Adapters;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Implementation
{
    public abstract class CrudAdapter<T, TI> : ICrudAdapter<T, TI>
        where T : BaseModel
        where TI : BaseUpsertModel
    {
        protected IMediator Mediator;
        public abstract Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
        public abstract Task<Result<List<T>>> GetAllAsync(CancellationToken cancellationToken);
        public abstract Task<Result<List<Guid>>> InsertAsync(List<TI> entities, CancellationToken cancellationToken);
        public abstract Task<Result<Guid>> InsertAsync(TI entity, CancellationToken cancellationToken);
        public abstract Task<Result<Guid>> UpdateAsync(Guid id, TI entity, CancellationToken cancellationToken);
    }
}