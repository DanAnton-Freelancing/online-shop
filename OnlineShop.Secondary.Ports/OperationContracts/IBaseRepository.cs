using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts;

public interface IBaseRepository
{
    Task<Result<List<T>>> GetAsync<T>(CancellationToken cancellationToken, Expression<Func<T, bool>> filter = null, 
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) where T : EditableEntity;

    Task<Result<T>> GetOneAsync<T>(Expression<Func<T, bool>> filter, CancellationToken cancellationToken,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) where T : EditableEntity;

}