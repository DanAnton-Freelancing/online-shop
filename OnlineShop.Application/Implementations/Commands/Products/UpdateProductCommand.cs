using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Extensions;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Products;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.Mappers;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Application.Implementations.Commands.Products;

public class UpdateProductCommand : IUpdateProductCommand
{
    public Guid Id { get; set; }
    public Product Data { get; set; }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<Guid>>
    {
        private readonly IWriterRepository _writerRepository;

        public UpdateProductCommandHandler(IWriterRepository writerRepository)
            => _writerRepository = writerRepository;

        public async Task<Result<Guid>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            => await _writerRepository.GetOneAsync<Product>(p => p.Id == request.Id, cancellationToken, null, p => p.Include(c => c.Category)
                                      .Include(c => c.Images))
                                      .MapAsync(p => p.MapToDomain())
                                      .AndAsync(p => p.Validate())
                                      .AndAsync(p => p.Hidrate(request.Data.MapToDomain()))
                                      .AndAsync(p => _writerRepository.AddAsync(p.MapToPorts(), cancellationToken))
                                      .AndAsync(p => _writerRepository.SaveAsync(p, cancellationToken));
    }
}