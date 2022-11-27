using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts
{
    public interface IBaseRepository<T>
        where T : EditableEntity
    {
        Task<Result<T>> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<List<T>>> GetAsync(CancellationToken cancellationToken);
        Task<Result<List<T>>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        Task<Result<T>> GetOneAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    }
}