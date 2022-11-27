using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Secondary.Adapters.Implementation.User
{
    public class UserReaderRepository : BaseReaderRepository<Ports.DataContracts.User>, IUserReaderRepository
    {
        public UserReaderRepository(DatabaseContext dbContext) : base(dbContext) { }

        public async Task<Result<Ports.DataContracts.User>> GetByUsernameAsync(string username, CancellationToken cancellationToken)
            => await GetOneAsync(u => u.Username.Equals(username), cancellationToken);
    }
}