using OnlineShop.Secondary.Ports.OperationContracts;

namespace OnlineShop.Secondary.Adapters.Implementation.Category;

public class ImageReaderRepository : BaseReaderRepository<Ports.DataContracts.Image>, IImageReaderRepository
{
    public ImageReaderRepository(DatabaseContext dbContext) : base(dbContext) { }
}