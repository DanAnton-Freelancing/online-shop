using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts;

public interface IWriterRepository : IBaseRepository
{
    Task<Result<Guid>> SaveAsync<T>(T entity, CancellationToken cancellationToken) where T : EditableEntity;
    Task<Result<List<Guid>>> SaveAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken) where T : EditableEntity;
    Task<Result<List<T>>> SaveAndGetAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken) where T : EditableEntity;
    Task<Result> DeleteAsync<T>(Guid id, CancellationToken cancellationToken) where T : EditableEntity;
    
    Task<Result<T>> SaveAndGetAsync<T>(T entity, CancellationToken cancellationToken) where T : EditableEntity;
}