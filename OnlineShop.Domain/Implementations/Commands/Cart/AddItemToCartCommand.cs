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

namespace OnlineShop.Domain.Implementations.Commands.Cart
{
    public class AddItemToCartCommand : IAddItemToCartCommand
    {
        public CartItem CartItem { get; set; }
        public Guid UserId { get; set; }

        public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, Result>
        {

            private readonly IUserCartWriterRepository _userCartWriterRepository;
            private readonly ICartItemWriterRepository _cartItemWriterRepository;
            private readonly IProductWriterRepository _productWriterRepository;
            private Product _product;

            public AddItemToCartCommandHandler(IUserCartWriterRepository userCartWriterRepository,
                                           ICartItemWriterRepository cartItemWriterRepository,
                                           IProductWriterRepository productWriterRepository)
            {
                _userCartWriterRepository = userCartWriterRepository;
                _cartItemWriterRepository = cartItemWriterRepository;
                _productWriterRepository = productWriterRepository;
            }

            public Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
                => _productWriterRepository.GetAsync(request.CartItem.ProductId, cancellationToken)
                                           .PipeAsync(i => i.IsAvailable())
                                           .PipeAsync(p => _product = p)
                                           .PipeAsync(p => request.CartItem.Hidrate(p))
                                           .AndAsync(i => _userCartWriterRepository.GetWithDetailsAsync(request.UserId, cancellationToken))
                                           .AndAsync(c => c.AddCartItem(request.CartItem))
                                           .PipeAsync(c => _cartItemWriterRepository.SaveAsync(c.CartItems, cancellationToken))
                                           .PipeAsync(c => _userCartWriterRepository.SaveAsync(c, cancellationToken))
                                           .AndAsync(p => _product.UpdateQuantity(0, request.CartItem.Quantity))
                                           .AndAsync(p => _productWriterRepository.SaveAsync(p, cancellationToken))
                                           .RemoveDataAsync();
        }
    }
}