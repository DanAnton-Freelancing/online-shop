﻿using System;
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

namespace OnlineShop.Secondary.Adapters.Implementation
{
    public abstract class BaseWriterRepository<T> : BaseRepository<T>, IBaseWriterRepository<T>
        where T : EditableEntity
    {
        protected BaseWriterRepository(DatabaseContext dbContext) : base(dbContext) { }
        
        public async Task<Result<Guid>> SaveAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                await InsertOrUpdateAsync(entity, cancellationToken);
                var count = await DbContext.SaveChangesAsync(cancellationToken);
                return count > 0
                    ? Result.Ok(entity.Id.GetValueOrDefault())
                    : Result.Error<Guid>(HttpStatusCode.NotModified, "[NotModified]", ErrorMessages.NotChanged);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Error<Guid>(HttpStatusCode.InternalServerError, "[DbError]", ErrorMessages.DbError);
            }
        }

        public async Task<Result<List<Guid>>> SaveAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            var entitiesList = entities?.ToList();
            if (entitiesList == null
                || entitiesList.Count <= 0)
                return Result.Error<List<Guid>>(HttpStatusCode.NotModified, "[NotModified]", ErrorMessages.NotChanged);

            try
            {
                foreach (var entity in entitiesList)
                   await InsertOrUpdateAsync(entity, cancellationToken); 
                var count = await DbContext.SaveChangesAsync(cancellationToken);
                return count > 0
                    ? Result.Ok(entitiesList.Select(e => e.Id.GetValueOrDefault()).ToList(),
                        HttpStatusCode.Created)
                    : Result.Error<List<Guid>>(HttpStatusCode.NotModified, "[NotModified]",
                        ErrorMessages.NotChanged);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Error<List<Guid>>(HttpStatusCode.InternalServerError, "[DbError]", ErrorMessages.DbError);
            }
        }

        public async Task<Result<List<T>>> SaveAndGetAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            var entitiesList = entities?.ToList();

            if (entitiesList == null
                || entitiesList.Count <= 0)
                return Result.Error<List<T>>(HttpStatusCode.NotModified, "[EntityNotProvided]",
                    ErrorMessages.NotProvided);

            try
            {
                foreach (var entity in entitiesList)
                   await InsertOrUpdateAsync(entity, cancellationToken);

                var count = await DbContext.SaveChangesAsync(cancellationToken);
                return count > 0
                    ? Result.Ok(entitiesList, HttpStatusCode.Created)
                    : Result.Error<List<T>>(HttpStatusCode.NotModified, "[NotModified]",
                        ErrorMessages.NotChanged);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Error<List<T>>(HttpStatusCode.InternalServerError, "[DbError]", ErrorMessages.DbError);
            }
        }


        public async Task<Result<T>> SaveAndGetAsync(T entity, CancellationToken cancellationToken)
        {
            var result = await SaveAndGetAsync(new List<T> { entity }, cancellationToken);
            return result.HasErrors
                ? Result.Error<T>(result.HttpStatusCode, result.ErrorMessage, result.ErrorMessage)
                : Result.Ok(result.Data.FirstOrDefault());
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var dbEntity = await DbSet.FindAsync(id);
                if (dbEntity == null)
                    return Result.Error(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

                DbContext.Set<T>().Remove(dbEntity);
                var count = await DbContext.SaveChangesAsync(cancellationToken);
                return count > 0
                           ? Result.Ok()
                           : Result.Error(HttpStatusCode.NotModified, "[NotDeleted]", ErrorMessages.NotChanged);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Error(HttpStatusCode.InternalServerError, "[DbError]", ErrorMessages.DbError);
            }
        }

        private async Task InsertOrUpdateAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity.Id == null)
            {
                await DbContext.AddAsync(entity, cancellationToken);
                return;
            }

            if (DbContext.Entry(entity).IsKeySet)
                DbContext.Entry(entity).State = EntityState.Modified;
            else
                DbContext.Update(entity); // To be investigated
        }
    }
}