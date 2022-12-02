using Microsoft.EntityFrameworkCore;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;

namespace OnlineShop.Secondary.Adapters.Implementation;

public abstract class BaseReaderRepository<T> : BaseRepository<T>, IBaseReaderRepository<T>
    where T : EditableEntity
{
    protected BaseReaderRepository(DatabaseContext dbContext) : base(dbContext) =>
        DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
}