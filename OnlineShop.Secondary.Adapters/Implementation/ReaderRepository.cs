using Microsoft.EntityFrameworkCore;
using OnlineShop.Secondary.Ports.OperationContracts;

namespace OnlineShop.Secondary.Adapters.Implementation;

public class ReaderRepository : BaseRepository, IReaderRepository
{
    public ReaderRepository(DatabaseContext dbContext) : base(dbContext) =>
        DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
}