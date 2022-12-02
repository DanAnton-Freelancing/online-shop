using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Secondary.Adapters.Implementation.Cart;

public class CartItemWriterRepository : BaseWriterRepository<CartItem>, ICartItemWriterRepository
{
    public CartItemWriterRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result<CartItem>> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken)
    {
        var cartItem = await DbContext.CartItems
            .Where(ci => ci.Id.Equals(id))
            .Include(u => u.Product)
            .FirstOrDefaultAsync(cancellationToken);
        return cartItem != null
            ? Result.Ok(cartItem)
            : Result.Error<CartItem>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

    }
}