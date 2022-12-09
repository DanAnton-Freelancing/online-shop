using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Application.Implementations.Commands.Categories;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Categories;

[TestClass]
public class UpdateCategoryCommandTests : BaseCommandTests<CategoryDb>
{

    private CategoryDb _firstCategoryDb;
    private UpdateCategoryCommand.UpdateCategoryCommandHandler _updateProductsCommandHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _updateProductsCommandHandler = new UpdateCategoryCommand.UpdateCategoryCommandHandler(WriterRepositoryMock.Object);
        Entities = CategoryFactory.Create();
        _firstCategoryDb = Entities.First().ToEntity();
    }

    [TestMethod]
    public async Task GivenCategoryAndId_WhenUpdateAsync_ThenShouldReturnId()
    {
        //Arrange
            
        WriterRepositoryMock.Setup(ls => ls.AddAsync(It.IsAny<CategoryDb>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_firstCategoryDb));

        WriterRepositoryMock.Setup(ls => ls.SaveAsync(It.IsAny<CategoryDb>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_firstCategoryDb.Id.GetValueOrDefault()));

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<CategoryDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<CategoryDb>, IOrderedQueryable<CategoryDb>>>(),
                It.IsAny<Func<IQueryable<CategoryDb>, IIncludableQueryable<CategoryDb, object>>>()))
            .ReturnsAsync(Result.Ok(_firstCategoryDb));
            
        //Act
        var result = await _updateProductsCommandHandler.Handle(new UpdateCategoryCommand
        {
            Data = _firstCategoryDb,
            Id = _firstCategoryDb.Id.GetValueOrDefault()
        }, CancellationToken.None);

        //Assert
        Assert.AreEqual(_firstCategoryDb.Id, result.Data);
    }
}