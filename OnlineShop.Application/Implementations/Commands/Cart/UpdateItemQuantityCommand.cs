using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Extensions;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Cart;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.Mappers;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Application.Implementations.Commands.Cart;

public class UpdateItemQuantityCommand : IUpdateItemQuantityCommand
{
    public Guid CartItemId { get; set; }

    public double Quantity { get; set; }

    public class UpdateItemQuantityCommandHandler : IRequestHandler<UpdateItemQuantityCommand, Result>
    {
        private readonly IWriterRepository _writerRepository;

        public UpdateItemQuantityCommandHandler(IWriterRepository writerRepository) 
            => _writerRepository = writerRepository;

        public async Task<Result> Handle(UpdateItemQuantityCommand request, CancellationToken cancellationToken)
            => await _writerRepository.GetOneAsync<CartItem>(c => c.Id == request.CartItemId, cancellationToken, null, 
                                                             c => c.Include(u => u.Product))
                                        .MapAsync(u => u.MapToDomain())
                                        .AndAsync(ci => ci.UpdateCartItem(request.Quantity))
                                      .AndAsync(ci => _writerRepository.AddAsync(ci.MapToPorts(), cancellationToken))
                                      .AndAsync(ci => _writerRepository.SaveAsync(ci, cancellationToken))
                                      .RemoveDataAsync();
    }
}