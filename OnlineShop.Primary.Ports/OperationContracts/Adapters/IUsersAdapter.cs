using System.Threading;
using System.Threading.Tasks;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Ports.OperationContracts.Adapters;

public interface IUsersAdapter
{
    Task<Result<string>> LoginAsync(LoginUserModel userModel, CancellationToken cancellationToken);
    Task<Result> RegisterAsync(RegisterUserModel userModel, CancellationToken cancellationToken);
}