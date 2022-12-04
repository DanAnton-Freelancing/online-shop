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

public class AddItemToCartCommand : IAddItemToCartCommand
{
    public CartItem CartItem { get; set; }
    public Guid UserId { get; set; }

    public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, Result>
    {
        private readonly IWriterRepository _writerRepository;
        private Product _product;

        public AddItemToCartCommandHandler(IWriterRepository writerRepository)
        {
            _writerRepository = writerRepository;
        }

        public Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken) 
            => _writerRepository.GetOneAsync<Product>(p => p.Id == request.CartItem.ProductId,
                    cancellationToken)
                                .PipeAsync(i => i.IsAvailable())
                                .PipeAsync(p => _product = p)
                                .PipeAsync(p => request.CartItem.Hidrate(p))
                                .AndAsync(i => _writerRepository.GetOneAsync<UserCart>(c => c.UserId == request.UserId,
                                    cancellationToken, null,
                                    a => a.Include(u => u.CartItems)
                                        .ThenInclude(u => u.Product)))
                                .AndAsync(c => c.AddCartItem(request.CartItem, _product))
                                .AndAsync(c => _writerRepository.AddAsync(c, cancellationToken))
                                .AndAsync(c => _writerRepository.SaveAsync(c, cancellationToken))
                                .RemoveDataAsync();
    }
}