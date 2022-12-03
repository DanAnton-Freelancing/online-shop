using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Extensions;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Cart;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Domain.Implementations.Commands.Cart;

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
                                    .AndAsync(ci => RemoveItemFromCart(ci, cancellationToken))
                                    .RemoveDataAsync();
        }

        private async Task<Result> RemoveItemFromCart(CartItem cartItem, CancellationToken cancellationToken)
        {
            var oldCartQuantity = cartItem.Quantity;

            if (Math.Abs(cartItem.Quantity - 1.0d) == 0)
            {
                //To Do: refactor _writerRepository to Commit changes only after all actions were done
                return await _writerRepository.DeleteAsync<CartItem>(cartItem.Id.GetValueOrDefault(), cancellationToken);
            }
            
            cartItem.Quantity -= 1;
            cartItem.Price -= cartItem.Product.Price.GetValueOrDefault();
            cartItem.Product.UpdateQuantity(oldCartQuantity, cartItem.Quantity);
            return await _writerRepository.SaveAsync(cartItem, cancellationToken);
        }
    }
}