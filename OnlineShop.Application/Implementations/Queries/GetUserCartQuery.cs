using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Aggregate;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.Mappers;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Application.Implementations.Queries;

public class GetUserCartQuery : IGetUserCartQuery
{
    public Guid UserId { get; set; }

    public class GetUserCartQueryHandler : IRequestHandler<GetUserCartQuery, Result<UserCartEntity>>
    {
        private readonly IReaderRepository _readerRepository;

        public GetUserCartQueryHandler(IReaderRepository readerRepository) 
            => _readerRepository = readerRepository;

        public async Task<Result<UserCartEntity>> Handle(GetUserCartQuery request, CancellationToken cancellationToken) =>
            await _readerRepository.GetOneAsync<UserCart>(c => c.UserId == request.UserId, 
                                                          cancellationToken, null,
                                                          c => c.Include(u => u.CartItems)
                                                                .ThenInclude(u => u.Product))
                .MapAsync(c => c.MapToDomain());
    }
}