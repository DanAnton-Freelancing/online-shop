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

public class AddItemToCartCommand : IAddItemToCartCommand
{
    public CartItemEntity CartItemEntity { get; set; }
    public Guid UserId { get; set; }

    public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, Result>
    {
        private readonly IWriterRepository _writerRepository;
        private ProductEntity _productEntity;

        public AddItemToCartCommandHandler(IWriterRepository writerRepository)
        {
            _writerRepository = writerRepository;
        }

        public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken) 
            => await _writerRepository.GetOneAsync<Product>(p => p.Id == request.CartItemEntity.ProductId,
                    cancellationToken)
                .MapAsync(p => p.MapToDomain())
                .PipeAsync(i => i.IsAvailable())
                .PipeAsync(p => _productEntity = p)
                .PipeAsync(p => request.CartItemEntity.Hidrate(p))
                .AndAsync(_ => _writerRepository.GetOneAsync<UserCart>(c => c.UserId == request.UserId,
                    cancellationToken, null,
                    a => a.Include(u => u.CartItems)
                        .ThenInclude(u => u.Product)))
                .MapAsync(p => p.MapToDomain())
                .AndAsync(c => c.AddCartItem(request.CartItemEntity, _productEntity))
                .AndAsync(c => _writerRepository.AddAsync(c.MapToPorts(), cancellationToken))
                .AndAsync(c => _writerRepository.SaveAsync(c, cancellationToken))
                .RemoveDataAsync();
    }
}