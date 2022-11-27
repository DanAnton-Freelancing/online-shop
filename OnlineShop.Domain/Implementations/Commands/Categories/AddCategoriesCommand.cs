using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Categories;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Domain.Implementations.Commands.Categories
{
    public class AddCategoriesCommand: IAddCategoriesCommand
    {
        public List<Category> Data { get; set; }

        public class AddCategoriesCommandHandler : IRequestHandler<AddCategoriesCommand, Result<List<Guid>>>
        {
            private readonly ICategoryWriterRepository _categoryWriterRepository;
            public AddCategoriesCommandHandler(ICategoryWriterRepository categoryWriterRepository)
            {
                // add Guard in the future
                _categoryWriterRepository = categoryWriterRepository;
            }

            public async Task<Result<List<Guid>>> Handle(AddCategoriesCommand request,
                CancellationToken cancellationToken)
                => await _categoryWriterRepository.SaveAsync(request.Data, cancellationToken);
        }
    }
}