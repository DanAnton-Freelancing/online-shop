using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts;

public interface IWriterRepository : IBaseRepository
{
    Task<Result<T>> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : Editable;
    Task<Result<List<T>>> AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken) where T : Editable;
    Task<Result> DeleteAsync<T>(Guid id, CancellationToken cancellationToken) where T : Editable;
    Task<Result<Guid>> SaveAsync<T>(T entity, CancellationToken cancellationToken) where T : Editable;
    Task<Result<List<Guid>>> SaveAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken)
        where T : Editable;
    Task<Result> SaveAsync(CancellationToken cancellationToken);

}