using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Secondary.Adapters.Implementation.Product
{
    public class ProductWriterRepository : BaseWriterRepository<Ports.DataContracts.Product>, IProductWriterRepository
    {
        public ProductWriterRepository(DatabaseContext dbContext) : base(dbContext) { }

        public async Task<Result<Ports.DataContracts.Product>> GetWithChildAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await DbSet.Where(p => p.Id == id)
                                    .Include(p => p.Category)
                                    .FirstOrDefaultAsync(cancellationToken);

            return entity != null
                ? Result.Ok(entity)
                : Result.Error<Ports.DataContracts.Product>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);
        }

        public async Task<Result> CheckIfIsUsedAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await DbSet.Where(p => p.Id == id)
                                    .Include(p => p.CartItem)
                                    .FirstOrDefaultAsync(cancellationToken);
            return entity.CartItem == null
                ? Result.Ok(entity)
                : Result.Error(HttpStatusCode.BadRequest, "[InUseNotDeleted]", ErrorMessages.InUseNotDeleted);
        }
    }
}