using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Application.Implementations.Commands.Cart;
using OnlineShop.Application.Implementations.Queries;
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

    public async Task<Result<UserCartModel>> GetWithDetailsAsync(Guid userId, CancellationToken cancellationToken)
        => await GuidValidator.ValidateAsync(userId)
            .AndAsync(() => _mediator.Send(new GetUserCartQuery { UserId = userId }, cancellationToken))
            .MapAsync(u => u.MapToPrimary());

    public async Task<Result> AddItemAsync(UpsertCartItemModel itemModel, Guid userId, CancellationToken cancellationToken)
        => await UpsertCartItemValidator.ValidateAsync(itemModel)
            .AndAsync(() => _mediator.Send(new AddItemToCartCommand { CartItemEntity = itemModel.MapToSecondary(), UserId = userId }, cancellationToken));


    public async Task<Result> RemoveItemAsync(Guid itemId, CancellationToken cancellationToken)
        => await GuidValidator.ValidateAsync(itemId)
            .AndAsync(() => _mediator.Send(new RemoveItemFromCartCommand { CartItemId = itemId }, cancellationToken));

    public async Task<Result> UpdateItemQuantityAsync(Guid itemId, double quantity, CancellationToken cancellationToken)
        => await GuidValidator.ValidateAsync(itemId)
            .AndAsync(() => UpsertCartItemValidator.ValidateQuantityAsync(quantity))
            .AndAsync(() => _mediator.Send(new UpdateItemQuantityCommand {CartItemId = itemId, Quantity = quantity},
                cancellationToken));
}