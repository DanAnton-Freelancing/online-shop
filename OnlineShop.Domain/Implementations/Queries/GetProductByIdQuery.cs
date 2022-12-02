using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Domain.Implementations.Queries;

public class GetProductByIdQuery : IGetProductByIdQuery
{
    public Guid Id { get; set; }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<Product>>
    {
        private readonly IProductReaderRepository _productReaderRepository;

        public GetProductByIdQueryHandler(IProductReaderRepository productReaderRepository)
        {
            _productReaderRepository = productReaderRepository;
        }

        public async Task<Result<Product>> Handle(GetProductByIdQuery request,
            CancellationToken cancellationToken) 
            => await _productReaderRepository.GetOneAsync(s => s.Id == request.Id, cancellationToken, null,
                                                          p => p.Include(s => s.Images));
    }
}