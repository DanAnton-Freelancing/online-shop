using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Application.Implementations.Commands.Users;
using OnlineShop.Application.Implementations.Queries;
using OnlineShop.Primary.Adapters.Mappers;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Primary.Ports.OperationContracts.Adapters;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Implementation;

public class UsersAdapter : IUsersAdapter
{
    private readonly IMediator _mediator;

    public UsersAdapter(IMediator mediator)
    {
        _mediator = mediator;
    }
  
    public async Task<Result<string>> LoginAsync(LoginUserModel userModel, CancellationToken cancellationToken)
        => await _mediator.Send(new LoginQuery { Username = userModel.Username, Password = userModel.Password }, cancellationToken);

    public async Task<Result> RegisterAsync(RegisterUserModel userModel, CancellationToken cancellationToken)
        => await _mediator.Send(new RegisterCommand {Data = userModel.MapToSecondary()}, cancellationToken);
}