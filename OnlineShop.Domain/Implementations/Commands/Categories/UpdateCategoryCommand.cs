using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Domain.Extensions;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Categories;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Domain.Implementations.Commands.Categories;

public class UpdateCategoryCommand : IUpdateCategoryCommand
{
    public Guid Id { get; set; }
    public Category Data { get; set; }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<Guid>>
    {
        private readonly ICategoryWriterRepository _categoryWriterRepository;

        public UpdateCategoryCommandHandler(ICategoryWriterRepository categoryWriterRepository)
        {
            _categoryWriterRepository = categoryWriterRepository;
        }

        public async Task<Result<Guid>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken) 
            => await _categoryWriterRepository.GetOneAsync(c => c.Id == request.Id, cancellationToken)
                                              .AndAsync(c => c.Validate())
                                              .AndAsync(c => c.Hidrate(request.Data))
                                              .AndAsync(c => _categoryWriterRepository.SaveAsync(c, cancellationToken));
    }
}