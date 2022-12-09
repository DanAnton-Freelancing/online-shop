using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineShop.Domain.Shared;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Secondary.Adapters.Implementation;

public abstract class BaseRepository: IBaseRepository
{
    protected readonly DatabaseContext DbContext;

    protected BaseRepository(DatabaseContext dbContext) 
        => DbContext = dbContext;

    public async Task<Result<List<T>>> GetAsync<T>(CancellationToken cancellationToken, 
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        where T : Editable

    {
        var queryable = GetAsync(filter, orderBy, include);

        var data = await queryable.ToListAsync(cancellationToken);

        return data.Count > 0
            ? Result.Ok(data)
            : Result.Error<List<T>>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);
    }
    public async Task<Result<T>> GetOneAsync<T>(
        Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) where T : Editable
    {
        var queryable = GetAsync(filter, orderBy, include);

        var data = await queryable.FirstOrDefaultAsync(cancellationToken);

        return data != null
            ? Result.Ok(data)
            : Result.Error<T>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);
    }

    private IQueryable<T> GetAsync<T>(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Func<IQueryable<T>, IIncludableQueryable<T, object>> include) 
        where T : Editable
    {
        IQueryable<T> queryable = DbContext.Set<T>();

        if (filter != null)
            queryable = queryable.Where(filter);

        if (include != null)
            queryable = include(queryable);

        if (orderBy != null)
            queryable = orderBy(queryable);
        return queryable;
    }
}