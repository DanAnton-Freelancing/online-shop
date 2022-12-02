using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Categories;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Domain.Implementations.Commands.Categories
{
    public class DeleteCategoryCommand : IDeleteCategoryCommand
    {
        public Guid Id { get; set; }

        public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
        {
            private readonly ICategoryWriterRepository _categoryWriterRepository;

            public DeleteCategoryCommandHandler(ICategoryWriterRepository categoryWriterRepository)
            {
                _categoryWriterRepository = categoryWriterRepository;
            }

            public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
                => await _categoryWriterRepository.GetOneAsync(c => c.Id == request.Id, cancellationToken)
                                                  .AndAsync(c => _categoryWriterRepository.CheckIfIsUsedAsync(c.Id.GetValueOrDefault(), cancellationToken))
                                                  .AndAsync(() => _categoryWriterRepository.DeleteAsync(request.Id, cancellationToken))
                                                  .RemoveDataAsync();
        }
    }
}