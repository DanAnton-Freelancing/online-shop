using OnlineShop.Secondary.Ports.OperationContracts;

namespace OnlineShop.Secondary.Adapters.Implementation.Category
{
    public class CategoryReaderRepository : BaseReaderRepository<Ports.DataContracts.Category>, ICategoryReaderRepository
    {
        public CategoryReaderRepository(DatabaseContext dbContext) : base(dbContext) { }
    }
}