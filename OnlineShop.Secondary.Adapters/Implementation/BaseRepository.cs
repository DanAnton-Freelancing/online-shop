using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Secondary.Adapters.Implementation;

public abstract class BaseRepository<T> : IBaseRepository<T>
    where T : EditableEntity
{
    protected readonly DatabaseContext DbContext;

    protected BaseRepository(DatabaseContext dbContext)
    {
        DbContext = dbContext;
    }

    protected DbSet<T> DbSet => DbContext.Set<T>();

    public async Task<Result<List<T>>> GetAsync(CancellationToken cancellationToken, 
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        var queryable = GetAsync(filter, orderBy, include);

        var data = await queryable.ToListAsync(cancellationToken);

        return data.Count > 0
            ? Result.Ok(data)
            : Result.Error<List<T>>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);
    }
    public async Task<Result<T>> GetOneAsync(
        Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        var queryable = GetAsync(filter, orderBy, include);

        var data = await queryable.FirstOrDefaultAsync(cancellationToken);

        return data != null
            ? Result.Ok(data)
            : Result.Error<T>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);
    }

    private IQueryable<T> GetAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
    {
        IQueryable<T> queryable = DbSet;

        if (filter != null)
            queryable = queryable.Where(filter);

        if (include != null)
            queryable = include(queryable);

        if (orderBy != null)
            queryable = orderBy(queryable);
        return queryable;
    }
}