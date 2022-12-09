using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Application.Implementations.Queries;

public class GetProductByIdQuery : IGetProductByIdQuery
{
    public Guid Id { get; set; }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<Product>>
    {
        private readonly IReaderRepository _readerRepository;

        public GetProductByIdQueryHandler(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public async Task<Result<Product>> Handle(GetProductByIdQuery request,
            CancellationToken cancellationToken) 
            => await _readerRepository.GetOneAsync<Product>(s => s.Id == request.Id, cancellationToken, null,
                                                            p => p.Include(s => s.Images));
    }
}