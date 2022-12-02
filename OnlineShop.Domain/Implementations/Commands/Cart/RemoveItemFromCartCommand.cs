using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Domain.Extensions;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Cart;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Domain.Implementations.Commands.Cart;

public class RemoveItemFromCartCommand : IRemoveItemFromCartCommand
{
    public Guid CartItemId { get; set; }

    public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand, Result>
    {
        private readonly ICartItemWriterRepository _cartItemWriterRepository;
        private readonly IProductWriterRepository _productWriterRepository;
        private double _oldCartQuantity;


        public RemoveItemFromCartCommandHandler(ICartItemWriterRepository cartItemWriterRepository, IProductWriterRepository productWriterRepository)
        {
            _cartItemWriterRepository = cartItemWriterRepository;
            _productWriterRepository = productWriterRepository;
        }

        public Task<Result> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
            => _cartItemWriterRepository.GetWithDetailsAsync(request.CartItemId, cancellationToken)
                .PipeAsync(ci => _oldCartQuantity = ci.Quantity)
                .PipeAsync(ci => _cartItemWriterRepository.DeleteAsync(request.CartItemId, cancellationToken))
                .PipeAsync(ci => ci.Product.UpdateQuantity(_oldCartQuantity, 0))
                .AndAsync(ci => _productWriterRepository.SaveAsync(ci.Product, cancellationToken))
                .RemoveDataAsync();
    }
}