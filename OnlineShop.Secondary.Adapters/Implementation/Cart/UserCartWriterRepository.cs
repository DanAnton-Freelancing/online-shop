using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Secondary.Adapters.Implementation.Cart;

public class UserCartWriterRepository : BaseWriterRepository<UserCart>, IUserCartWriterRepository
{
    public UserCartWriterRepository(DatabaseContext dbContext) : base(dbContext) { }

    public async Task<Result<UserCart>> GetWithDetailsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userCart = await UserCartDetailsFactory.Create(DbContext,userId, cancellationToken);

        return userCart != null
            ? Result.Ok(userCart)
            : Result.Error<UserCart>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);
    }
}