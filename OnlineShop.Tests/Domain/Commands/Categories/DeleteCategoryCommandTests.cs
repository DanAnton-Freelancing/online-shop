using System;
using System.Linq.Expressions;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Commands.Categories;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Secondary.Ports.OperationContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Categories
{
    [TestClass]
    public class DeleteCategoryCommandTests : BaseCommandTests<Category, ICategoryWriterRepository>
    {
        private DeleteCategoryCommand.DeleteCategoryCommandHandler _deleteProductCommandHandler;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
            _deleteProductCommandHandler =
                new DeleteCategoryCommand.DeleteCategoryCommandHandler(WriterRepositoryMock.Object);
            Entities = CategoryFactory.Create();
        }

        [TestMethod]
        public async Task GivenCategoryId_WhenDeleteAsync_ThenShouldReturnOk()
        {
            //Arrange
            var upsertProduct = CategoryFactory.CreateUpsert();

            WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Category, bool>>>(),
                    CancellationToken.None, It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                    It.IsAny<Func<IQueryable<Category>, IIncludableQueryable<Category, object>>>()))
                .ReturnsAsync(Result.Ok(upsertProduct));

            WriterRepositoryMock.Setup(ls => ls.CheckIfIsUsedAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(Result.Ok());


            WriterRepositoryMock.Setup(ls => ls.DeleteAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(Result.Ok());

            //Act
            var result = await _deleteProductCommandHandler.Handle(new DeleteCategoryCommand
            {
                Id = upsertProduct.Id.GetValueOrDefault()
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.Success);
        }


        [TestMethod]
        public async Task GivenWrongCategoryId_WhenDeleteAsync_ThenShouldReturnError()
        {
            //Arrange
            var upsertCategory = CategoryFactory.CreateUpsert();
            var error = Result.Error<Category>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

            WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Category, bool>>>(),
                    CancellationToken.None, It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                    It.IsAny<Func<IQueryable<Category>, IIncludableQueryable<Category, object>>>()))
                .ReturnsAsync(error);
            
            WriterRepositoryMock.Setup(ls => ls.CheckIfIsUsedAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(Result.Ok());

            WriterRepositoryMock.Setup(ls => ls.DeleteAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(Result.Ok());

            //Act
            var result = await _deleteProductCommandHandler.Handle(new DeleteCategoryCommand
            {
                Id = upsertCategory.Id.GetValueOrDefault()
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual(result.ErrorMessage, error.ErrorMessage);
            Assert.AreEqual(result.HttpStatusCode, error.HttpStatusCode);
        }


        [TestMethod]
        public async Task GivenInUseProductId_WhenDeleteProduct_ThenShouldReturnError()
        {
            //Arrange
            var upsertCategory = CategoryFactory.CreateUpsert();
            var error = Result.Error<Category>(HttpStatusCode.BadRequest, "[InUseNotDeleted]", ErrorMessages.InUseNotDeleted);

            WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Category, bool>>>(),
                    CancellationToken.None, It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                    It.IsAny<Func<IQueryable<Category>, IIncludableQueryable<Category, object>>>()))
                .ReturnsAsync(Result.Ok(upsertCategory));

            WriterRepositoryMock.Setup(ls => ls.CheckIfIsUsedAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(error);


            WriterRepositoryMock.Setup(ls => ls.DeleteAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(Result.Ok());

            //Act
            var result = await _deleteProductCommandHandler.Handle(new DeleteCategoryCommand
            {
                Id = upsertCategory.Id.GetValueOrDefault()
            }, CancellationToken.None);

            //Assert
            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual(result.ErrorMessage, error.ErrorMessage);
            Assert.AreEqual(result.HttpStatusCode, error.HttpStatusCode);
        }
    }
}