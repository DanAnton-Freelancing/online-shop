using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Products;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Domain.Implementations.Commands.Products;

public class AddProductsCommand : IAddProductsCommand
{
    public Product Data { get; set; }
        
    public class AddProductsCommandHandler : IRequestHandler<AddProductsCommand, Result<Guid>>
    {
        private readonly IProductWriterRepository _productWriterRepository;

        public AddProductsCommandHandler(IProductWriterRepository productWriterRepository)
        {
            // add guard for repository 
            _productWriterRepository = productWriterRepository;
        }

        public async Task<Result<Guid>> Handle(AddProductsCommand request, CancellationToken cancellationToken) 
            => await _productWriterRepository.SaveAsync(request.Data, cancellationToken);
    }
}