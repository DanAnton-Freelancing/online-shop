using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Secondary.Adapters.Implementation
{
    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : EditableEntity
    {
        protected readonly DatabaseContext DbContext;

        protected BaseRepository(DatabaseContext dbContext) => DbContext = dbContext;

        protected DbSet<T> DbSet => DbContext.Set<T>();

        public async Task<Result<T>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var data = await DbSet.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
            return data != null
                ? Result.Ok(data)
                : Result.Error<T>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);
        }

        public async Task<Result<List<T>>> GetAsync(CancellationToken cancellationToken)
            => await GetAsync(e => true, cancellationToken);

        public async Task<Result<List<T>>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            var data = await DbSet.Where(predicate).ToListAsync(cancellationToken);
            return data.Count > 0
                ? Result.Ok(data)
                : Result.Error<List<T>>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);
        }

        public async Task<Result<T>> GetOneAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            var data = await DbSet.FirstOrDefaultAsync(predicate, cancellationToken);
            return data != null
                ? Result.Ok(data)
                : Result.Error<T>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);
        }
    }
}