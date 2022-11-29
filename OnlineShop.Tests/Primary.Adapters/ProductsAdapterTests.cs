using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Primary.Adapters.Implementation;
using OnlineShop.Primary.Adapters.Mappers;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;
using System.Threading.Tasks;
using System.Threading;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Products;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;

namespace OnlineShop.Tests.Primary.Adapters
{
    [TestClass]
    public class ProductsAdapterTests : BaseTests
    {
        private secondaryPorts.Product _firstProduct;
        private List<secondaryPorts.Product> _products;
        private ProductsAdapter _productsAdapter;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            _products = ProductFactory.Create();
            _productsAdapter = new ProductsAdapter(MediatorMock.Object);
            _firstProduct = _products.First().ToEntity();
        }

        [TestMethod]
        public async Task WhenGetAsyncProducts_ThenShouldReturnProducts()
        {
            //Arrange
            MediatorMock.Setup(m => m.Send(It.IsAny<IGetProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(_products));

            //Act
            var result = await _productsAdapter.GetAllAsync(CancellationToken.None);

            //Assert
            Assert.IsTrue(ModelAssertionsUtils<primaryPorts.Product>.AreListsEqual(result.Data, _products.MapToPrimary()));
        }

        [TestMethod]
        public async Task GivenProducts_WhenInsertAsync_ThenShouldReturnIds()
        {
            //Arrange
            var locationIds = _products.Select(l => l.Id.GetValueOrDefault()).ToList();

            MediatorMock.Setup(m => m.Send(It.IsAny<IAddProductsCommand>(), CancellationToken.None))
                .ReturnsAsync(Result.Ok(locationIds));

            var upsertCategories = ProductFactory.CreateUpsertModels();

            //Act
            var result = await _productsAdapter.InsertAsync(upsertCategories, CancellationToken.None);

            //Assert
            Assert.AreEqual(locationIds, result.Data);
        }

        [TestMethod]
        public async Task GivenProductAndId_WhenUpdateAsync_ThenShouldReturnId()
        {
            //Arrange
            var upsertCategory = ProductFactory.CreateUpsertModel();

            MediatorMock.Setup(ls => ls.Send(It.IsAny<IUpdateProductCommand>(), CancellationToken.None))
                .ReturnsAsync(Result.Ok(_firstProduct.Id.GetValueOrDefault()));

            //Act
            var result = await _productsAdapter.UpdateAsync(_firstProduct.Id.GetValueOrDefault(), upsertCategory, CancellationToken.None);

            //Assert
            Assert.AreEqual(_firstProduct.Id.GetValueOrDefault(), result.Data);
        }

        [TestMethod]
        public async Task GivenProductId_WhenDeleteAsync_ThenShouldReturnOk()
        {
            //Arrange

            MediatorMock.Setup(ls => ls.Send(It.IsAny<IDeleteProductCommand>(), CancellationToken.None))
                .ReturnsAsync(Result.Ok());

            //Act
            var result = await _productsAdapter.DeleteAsync(_firstProduct.Id.GetValueOrDefault(),CancellationToken.None);

            //Assert
            Assert.IsTrue(result.Success);
        }
    }
}