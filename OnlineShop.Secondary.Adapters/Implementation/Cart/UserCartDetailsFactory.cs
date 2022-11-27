using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Adapters.Implementation.Cart
{
    internal static class UserCartDetailsFactory
    {

        public static async Task<UserCart> Create(DatabaseContext dbContext, Guid userId, CancellationToken cancellationToken)
        {
            return await dbContext.UserCarts
                                  .Where(u => u.UserId.Equals(userId))
                                  .Include(u => u.CartItems)
                                  .ThenInclude(u => u.Product)
                                  .FirstOrDefaultAsync(cancellationToken);
        }
    }
}