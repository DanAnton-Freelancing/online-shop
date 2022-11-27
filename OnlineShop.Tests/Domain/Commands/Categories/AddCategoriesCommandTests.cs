using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Commands.Categories;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Categories
{
    [TestClass]
    public class AddCategoriesCommandTests : BaseCommandTests<Category, ICategoryWriterRepository>
    {
        private AddCategoriesCommand.AddCategoriesCommandHandler _addProductsCommandHandler;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            _addProductsCommandHandler = new AddCategoriesCommand.AddCategoriesCommandHandler(WriterRepositoryMock.Object);
            Entities = CategoryFactory.Create();
            Entities.ForEach(p => p.ToEntity());
        }

        [TestMethod]
        public async Task GivenCategories_WhenInsertAsync_ThenShouldReturnIds()
        {
            //Arrange
            var productIds = Entities.Select(l => l.Id.GetValueOrDefault()).ToList();

            WriterRepositoryMock.Setup(ls => ls.SaveAsync(It.IsAny<List<Category>>(),CancellationToken.None))
                .ReturnsAsync(Result.Ok(productIds));

            //Act
            var result = await _addProductsCommandHandler.Handle(new AddCategoriesCommand
            {
                Data = Entities
            }, CancellationToken.None);

            //Assert
            Assert.AreEqual(productIds, result.Data);
        }
    }
}