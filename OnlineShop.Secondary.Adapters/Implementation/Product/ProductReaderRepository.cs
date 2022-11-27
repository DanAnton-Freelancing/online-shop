using OnlineShop.Secondary.Ports.OperationContracts;

namespace OnlineShop.Secondary.Adapters.Implementation.Product
{
    public class ProductReaderRepository : BaseReaderRepository<Ports.DataContracts.Product>, IProductReaderRepository
    {
        public ProductReaderRepository(DatabaseContext dbContext) : base(dbContext) { }
    }
}