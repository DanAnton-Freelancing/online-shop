using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Commands.Products;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Products
{
    [TestClass]
    public class AddProductsCommandTests : BaseCommandTests<Product, IProductWriterRepository>
    {
        private AddProductsCommand.AddProductsCommandHandler _addProductsCommandHandler;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            _addProductsCommandHandler = new AddProductsCommand.AddProductsCommandHandler(WriterRepositoryMock.Object);
            Entities = ProductFactory.Create();
            Entities.ForEach(p => p.ToEntity());
        }

        [TestMethod]
        public async Task GivenProducts_WhenInsertAsync_ThenShouldReturnIds()
        {
            //Arrange
            var productIds = Entities.Select(l => l.Id.GetValueOrDefault()).ToList();

            WriterRepositoryMock.Setup(ls => ls.SaveAsync(It.IsAny<List<Product>>(),CancellationToken.None))
                .ReturnsAsync(Result.Ok(productIds));

            //Act
            var result = await _addProductsCommandHandler.Handle(new AddProductsCommand
            {
                Data = Entities
            }, CancellationToken.None);

            //Assert
            Assert.AreEqual(productIds, result.Data);
        }
    }
}