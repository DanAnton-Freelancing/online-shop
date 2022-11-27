using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts
{
    public interface IBaseWriterRepository<T> : IBaseRepository<T>
        where T : EditableEntity
    {
        Task<Result<Guid>> SaveAsync(T entity, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<List<Guid>>> SaveAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        Task<Result<List<T>>> SaveAndGetAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        Task<Result<T>> SaveAndGetAsync(T entity, CancellationToken cancellationToken);
    }
}