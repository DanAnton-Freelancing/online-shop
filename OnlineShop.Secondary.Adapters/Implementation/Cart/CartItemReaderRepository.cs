using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;

namespace OnlineShop.Secondary.Adapters.Implementation.Cart;

public class CartItemReaderRepository : BaseReaderRepository<CartItem>, ICartItemReaderRepository
{
    public CartItemReaderRepository(DatabaseContext dbContext) : base(dbContext) { }
}