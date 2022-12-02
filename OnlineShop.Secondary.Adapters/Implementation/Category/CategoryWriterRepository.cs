using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Secondary.Adapters.Implementation.Category;

public class CategoryWriterRepository : BaseWriterRepository<Ports.DataContracts.Category>, ICategoryWriterRepository
{
    public CategoryWriterRepository(DatabaseContext dbContext) : base(dbContext) { }

    public async Task<Result> CheckIfIsUsedAsync(Guid id, CancellationToken cancellationToken)
    {
        var category = await DbSet.Where(c => c.Id.Equals(id))
            .Include(c => c.Products)
            .FirstOrDefaultAsync(cancellationToken);

        return category?.Products.Count > 0 
            ? Result.Error(HttpStatusCode.BadRequest, "[InUseNotDeleted]", ErrorMessages.InUseNotDeleted) 
            : Result.Ok();
    }
}