using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Queries
{
    [TestClass]
    public class GetProductsQueryTests: BaseTests
    {
        private Mock<IProductReaderRepository> _productReaderRepositoryMock;
        private List<Product> _products;
        private GetProductsQuery.GetProductsQueryHandler _getProductsQueryHandler;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            _productReaderRepositoryMock = new Mock<IProductReaderRepository>(MockBehavior.Strict) { CallBase = true };
            _products = ProductFactory.Create();
            _getProductsQueryHandler = new GetProductsQuery.GetProductsQueryHandler(_productReaderRepositoryMock.Object);

        }

        [TestMethod]
        public async Task WhenGetProducts_ThenShouldReturnProducts()
        {
            //Arrange
            _productReaderRepositoryMock.Setup(ls => ls.GetAsync(CancellationToken.None))
                .ReturnsAsync(Result.Ok(_products));

            //Act
            var result = await _getProductsQueryHandler.Handle(new GetProductsQuery(), CancellationToken.None);

            //Assert
            Assert.IsTrue(EntitiesAssertionsUtils<Product>.AreListsEqual(result.Data, _products));

        }
    }
}