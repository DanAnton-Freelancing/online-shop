﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Domain.Implementations.Commands.Users;
using OnlineShop.Domain.Implementations.Queries;
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
  
    public async Task<Result<string>> LoginAsync(LoginUser user, CancellationToken cancellationToken)
        => await _mediator.Send(new LoginQuery { Username = user.Username, Password = user.Password }, cancellationToken);

    public async Task<Result> RegisterAsync(RegisterUser user, CancellationToken cancellationToken)
        => await _mediator.Send(new RegisterCommand {Data = user.MapToSecondary()}, cancellationToken);
}