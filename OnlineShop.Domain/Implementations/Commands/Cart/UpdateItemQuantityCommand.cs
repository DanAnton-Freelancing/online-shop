using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Domain.Extensions;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Cart;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Domain.Implementations.Commands.Cart;

public class UpdateItemQuantityCommand : IUpdateItemQuantityCommand
{
    public Guid CartItemId { get; set; }

    public double Quantity { get; set; }

    public class UpdateItemQuantityCommandHandler : IRequestHandler<UpdateItemQuantityCommand, Result>
    {
        private readonly ICartItemWriterRepository _cartItemWriterRepository;
        private readonly IProductWriterRepository _productWriterRepository;
        private Product _product;
        private double _oldCartQuantity;


        public UpdateItemQuantityCommandHandler(ICartItemWriterRepository cartItemWriterRepository, IProductWriterRepository productWriterRepository)
        {
            _cartItemWriterRepository = cartItemWriterRepository;
            _productWriterRepository = productWriterRepository;
        }

        public async Task<Result> Handle(UpdateItemQuantityCommand request, CancellationToken cancellationToken)
            => await _cartItemWriterRepository.GetWithDetailsAsync(request.CartItemId, cancellationToken)
                .PipeAsync(ci => _product = ci.Product)
                .PipeAsync(ci => _oldCartQuantity = ci.Quantity)
                .AndAsync(ci => ci.UpdateCartItem(request.Quantity))
                .AndAsync(ci => _cartItemWriterRepository.SaveAndGetAsync(ci, cancellationToken))
                .AndAsync(ci => _product.UpdateQuantity(_oldCartQuantity, ci.Quantity))
                .AndAsync(p => _productWriterRepository.SaveAsync(p, cancellationToken))
                .RemoveDataAsync();
    }
}