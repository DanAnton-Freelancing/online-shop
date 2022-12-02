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
        private readonly IImageReaderRepository _imageReaderRepository;

        public GetImageByIdQueryHandler(IImageReaderRepository imageReaderRepository)
        {
            _imageReaderRepository = imageReaderRepository;
        }

        public async Task<Result<Image>> Handle(GetImageByIdQuery request, CancellationToken cancellationToken) 
            => await _imageReaderRepository.GetOneAsync(i => request.ImageId == i.Id, cancellationToken);
    }
}