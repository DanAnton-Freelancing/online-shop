using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Commands.Categories;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Commands.Categories;

[TestClass]
public class UpdateCategoryCommandTests : BaseCommandTests<Category>
{

    private Category _firstCategory;
    private UpdateCategoryCommand.UpdateCategoryCommandHandler _updateProductsCommandHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _updateProductsCommandHandler = new UpdateCategoryCommand.UpdateCategoryCommandHandler(WriterRepositoryMock.Object);
        Entities = CategoryFactory.Create();
        _firstCategory = Entities.First().ToEntity();
    }

    [TestMethod]
    public async Task GivenCategoryAndId_WhenUpdateAsync_ThenShouldReturnId()
    {
        //Arrange
            
        WriterRepositoryMock.Setup(ls => ls.AddAsync(It.IsAny<Category>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_firstCategory));

        WriterRepositoryMock.Setup(ls => ls.SaveAsync(It.IsAny<Category>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_firstCategory.Id.GetValueOrDefault()));

        WriterRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Category, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<Func<IQueryable<Category>, IIncludableQueryable<Category, object>>>()))
            .ReturnsAsync(Result.Ok(_firstCategory));
            
        //Act
        var result = await _updateProductsCommandHandler.Handle(new UpdateCategoryCommand
        {
            Data = _firstCategory,
            Id = _firstCategory.Id.GetValueOrDefault()
        }, CancellationToken.None);

        //Assert
        Assert.AreEqual(_firstCategory.Id, result.Data);
    }
}