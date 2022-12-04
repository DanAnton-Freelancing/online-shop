using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Secondary.Adapters.Implementation;

public class WriterRepository : BaseRepository, IWriterRepository
{
    public WriterRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result<T>> AddAsync<T>(T entity, CancellationToken cancellationToken) where T : EditableEntity
    {
        try
        {
            return await InsertOrUpdateAsync(entity, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Error<T>(HttpStatusCode.InternalServerError, "[DbError]", ErrorMessages.DbError);
        }
    }

    public async Task<Result<List<T>>> AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken)
        where T : EditableEntity
    {
        var entitiesList = entities?.ToList();
        if (entitiesList is not { Count: > 0 })
            return Result.Error<List<T>>(HttpStatusCode.NotModified, "[NotModified]", ErrorMessages.NotChanged);
        try
        {
            foreach (var entity in entitiesList)
                await InsertOrUpdateAsync(entity, cancellationToken);

            return Result.Ok(entitiesList);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Error<List<T>>(HttpStatusCode.InternalServerError, "[DbError]", ErrorMessages.DbError);
        }
    }

    public async Task<Result<Guid>> SaveAsync<T>(T entity, CancellationToken cancellationToken) where T : EditableEntity
    {
        var count = await DbContext.SaveChangesAsync(cancellationToken);
        return count > 0
            ? Result.Ok(entity.Id.GetValueOrDefault())
            : Result.Error<Guid>(HttpStatusCode.NotModified, "[NotModified]", ErrorMessages.NotChanged);
    }
    public async Task<Result> SaveAsync(CancellationToken cancellationToken)
    {
        var count = await DbContext.SaveChangesAsync(cancellationToken);
        return count > 0
            ? Result.Ok()
            : Result.Error<Guid>(HttpStatusCode.NotModified, "[NotDeleted]", ErrorMessages.NotChanged);
    }

    public async Task<Result<List<Guid>>> SaveAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken) where T : EditableEntity
    {
        var count = await DbContext.SaveChangesAsync(cancellationToken);
        return count > 0
            ? Result.Ok(entities.Select(e => e.Id.GetValueOrDefault()).ToList(),
                HttpStatusCode.Created)
            : Result.Error<List<Guid>>(HttpStatusCode.NotModified, "[NotModified]",
                ErrorMessages.NotChanged);
    }

    public async Task<Result> DeleteAsync<T>(Guid id, CancellationToken cancellationToken) where T : EditableEntity
    {
        try
        {
            var dbEntity = await DbContext.Set<T>().FindAsync(id);
            if (dbEntity == null)
                return Result.Error(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

            DbContext.Set<T>().Remove(dbEntity);
           return Result.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Error(HttpStatusCode.InternalServerError, "[DbError]", ErrorMessages.DbError);
        }
    }

    private async Task<Result<T>> InsertOrUpdateAsync<T>(T entity, CancellationToken cancellationToken)
        where T : EditableEntity
    {
        if (entity.Id == null)
        {
            await DbContext.SingleInsertAsync(entity, cancellationToken);
            return Result.Ok(entity);
        }

        if (DbContext.Entry(entity).IsKeySet)
            DbContext.Entry(entity).State = EntityState.Modified;
        else
            await DbContext.SingleUpdateAsync(entity, cancellationToken);
        return Result.Ok(entity);
    }
}