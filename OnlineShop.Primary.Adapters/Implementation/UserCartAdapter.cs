using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Domain.Implementations.Commands.Cart;
using OnlineShop.Domain.Implementations.Queries;
using OnlineShop.Primary.Adapters.Mappers;
using OnlineShop.Primary.Adapters.Validators;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Primary.Ports.OperationContracts.Adapters;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;
using OnlineShop.Shared.Ports.Validators;

namespace OnlineShop.Primary.Adapters.Implementation;

public class UserCartAdapter : IUserCartAdapter
{
    private readonly IMediator _mediator;

    public UserCartAdapter(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<UserCart>> GetWithDetailsAsync(Guid userId, CancellationToken cancellationToken)
        => await GuidValidator.ValidateAsync(userId, cancellationToken)
            .AndAsync(() => _mediator.Send(new GetUserCartQuery { userId = userId }, cancellationToken))
            .MapAsync(u => u.MapToPrimary());

    public async Task<Result> AddItemAsync(UpsertCartItem item, Guid userId, CancellationToken cancellationToken)
        => await UpsertCartItemValidator.ValidateAsync(item)
            .AndAsync(() => _mediator.Send(new AddItemToCartCommand { CartItem = item.MapToSecondary(), UserId = userId }, cancellationToken));


    public async Task<Result> RemoveItemAsync(Guid itemId, CancellationToken cancellationToken)
        => await GuidValidator.ValidateAsync(itemId, cancellationToken)
            .AndAsync(() => _mediator.Send(new RemoveItemFromCartCommand { CartItemId = itemId }, cancellationToken));

    public async Task<Result> UpdateItemQuantityAsync(Guid itemId, double quantity, CancellationToken cancellationToken)
        => await GuidValidator.ValidateAsync(itemId, cancellationToken)
            .AndAsync(() => UpsertCartItemValidator.ValidateQuantityAsync(quantity))
            .AndAsync(() => _mediator.Send(new UpdateItemQuantityCommand {CartItemId = itemId, Quantity = quantity},
                cancellationToken));
}