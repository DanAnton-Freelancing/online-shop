using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Domain.Extensions;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Products;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;
using Product = OnlineShop.Secondary.Ports.DataContracts.Product;

namespace OnlineShop.Domain.Implementations.Commands.Products;

public class UpdateProductCommand : IUpdateProductCommand
{
    public Guid Id { get; set; }
    public Product Data { get; set; }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<Guid>>
    {
        private readonly IProductWriterRepository _productWriterRepository;

        public UpdateProductCommandHandler(IProductWriterRepository productWriterRepository)
            => _productWriterRepository = productWriterRepository;

        public async Task<Result<Guid>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            => await _productWriterRepository.GetWithChildAsync(request.Id, cancellationToken)
                .AndAsync(p => p.Validate())
                .AndAsync(p => p.Hidrate(request.Data))
                .AndAsync(p => _productWriterRepository.SaveAsync(p, cancellationToken));
    }
}