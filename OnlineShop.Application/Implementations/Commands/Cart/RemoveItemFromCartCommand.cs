using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Extensions;
using OnlineShop.Domain.Aggregate;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Cart;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.Mappers;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Application.Implementations.Commands.Cart;

public class RemoveItemFromCartCommand : IRemoveItemFromCartCommand
{
    public Guid CartItemId { get; set; }

    public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand, Result>
    {
        private readonly IWriterRepository _writerRepository;


        public RemoveItemFromCartCommandHandler(IWriterRepository writerRepository)
        {
            _writerRepository = writerRepository;
        }

        public Task<Result> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
        {
            return _writerRepository.GetOneAsync<CartItem>(c => c.Id == request.CartItemId, cancellationToken,
                    null,
                    c => c.Include(u => u.Product))
                .AndAsync(ci => RemoveItemFromCart(ci.MapToDomain(), cancellationToken))
                .RemoveDataAsync();
        }

        private async Task<Result> RemoveItemFromCart(CartItemEntity cartItemEntity, CancellationToken cancellationToken)
        {
            var oldCartQuantity = cartItemEntity.Quantity;

            if (Math.Abs(cartItemEntity.Quantity - 1.0d) == 0)
            {
                var product = cartItemEntity.ProductEntity;
                await _writerRepository.DeleteAsync<CartItem>(cartItemEntity.Id.GetValueOrDefault(), cancellationToken);

                product.UpdateQuantity(oldCartQuantity, 0);
                await _writerRepository.AddAsync(product.MapToPorts(), cancellationToken);
                return await _writerRepository.SaveAsync(cancellationToken);
            }

            cartItemEntity.Quantity -= 1;
            cartItemEntity.Price -= cartItemEntity.ProductEntity.Price.GetValueOrDefault();
            cartItemEntity.ProductEntity.UpdateQuantity(oldCartQuantity, cartItemEntity.Quantity);

            return await _writerRepository.AddAsync(cartItemEntity.MapToPorts(), cancellationToken)
                .AndAsync(_ => _writerRepository.SaveAsync(cancellationToken));
        }
    }
}