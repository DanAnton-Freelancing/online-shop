using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts
{
    public interface IUserWriterRepository : IBaseWriterRepository<User>
    {
        Task<Result> UserNotExistsAsync(User user, CancellationToken cancellationToken);
    }
}