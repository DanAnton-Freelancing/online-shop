using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Domain.Implementations.Queries;

public class GetImageByIdQuery : IGetImageByIdQuery
{
    public Guid ImageId { get; set; }

    public class GetImageByIdQueryHandler : IRequestHandler<GetImageByIdQuery, Result<Image>>
    {
        private readonly IReaderRepository _readerRepository;

        public GetImageByIdQueryHandler(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public async Task<Result<Image>> Handle(GetImageByIdQuery request, CancellationToken cancellationToken) 
            => await _readerRepository.GetOneAsync<Image>(i => request.ImageId == i.Id, cancellationToken);
    }
}