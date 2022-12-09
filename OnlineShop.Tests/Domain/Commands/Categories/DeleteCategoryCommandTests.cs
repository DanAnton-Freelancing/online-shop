using System;
using System.Linq.Expressions;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Application.Implementations.Commands.Categories;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Resources;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Categories;

[TestClass]
public class DeleteCategoryCommandTests : BaseCommandTests<CategoryDb>
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

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<CategoryDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
                It.IsAny<Func<IQueryable<CategoryDb>, IIncludableQueryable<CategoryDb, object>>>()))
            .ReturnsAsync(Result.Ok(upsertProduct));
        

        WriterRepositoryMock.Setup(ls => ls.DeleteAsync<CategoryDb>(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        WriterRepositoryMock.Setup(ls => ls.SaveAsync(CancellationToken.None))
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
        var error = Result.Error<CategoryDb>(HttpStatusCode.NotFound, "[NotFound]", ErrorMessages.NotFound);

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<CategoryDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
                It.IsAny<Func<IQueryable<CategoryDb>, IIncludableQueryable<CategoryDb, object>>>()))
            .ReturnsAsync(error);

        WriterRepositoryMock.Setup(ls => ls.DeleteAsync<CategoryDb>(It.IsAny<Guid>(), CancellationToken.None))
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
    public async Task GivenInUseCategoryId_WhenDeleteCategory_ThenShouldReturnError()
    {
        //Arrange
        var upsertCategory = CategoryFactory.CreateUpsert();
        upsertCategory.Products = ProductFactory.Create();
        var error = Result.Error<CategoryDb>(HttpStatusCode.BadRequest, "[InUseNotDeleted]", ErrorMessages.InUseNotDeleted);

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<CategoryDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
                It.IsAny<Func<IQueryable<CategoryDb>, IIncludableQueryable<CategoryDb, object>>>()))
            .ReturnsAsync(Result.Ok(upsertCategory));

        WriterRepositoryMock.Setup(ls => ls.DeleteAsync<CategoryDb>(It.IsAny<Guid>(), CancellationToken.None))
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