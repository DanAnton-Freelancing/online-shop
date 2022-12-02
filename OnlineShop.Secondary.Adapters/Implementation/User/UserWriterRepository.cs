using System.Net;
using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;

namespace OnlineShop.Secondary.Adapters.Implementation.User;

public class UserWriterRepository : BaseWriterRepository<Ports.DataContracts.User>, IUserWriterRepository
{
    public UserWriterRepository(DatabaseContext dbContext) : base(dbContext) { }


    public async Task<Result> UserNotExistsAsync(Ports.DataContracts.User user, CancellationToken cancellationToken)
    {
        var entity = await GetOneAsync(u => u.Username.Equals(user.Username) ||
                                            u.Email.Equals(user.Email), cancellationToken);
        return entity.Success
            ? Result.Error<bool>(HttpStatusCode.Conflict, "[AlreadyExist]", ErrorMessages.UserAlreadyExist)
            : Result.Ok();
    }
}