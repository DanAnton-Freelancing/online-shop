using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Secondary.Ports.OperationContracts
{
    public interface IUserReaderRepository : IBaseReaderRepository<User>
    {
        Task<Result<User>> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    }
}