using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Primary.Adapters.Implementation;
using OnlineShop.Primary.Adapters.Mappers;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Categories;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;
using OnlineShop.Secondary.Ports.Mappers;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;

namespace OnlineShop.Tests.Primary.Adapters;

[TestClass]
public class CategoriesAdapterTests : BaseTests
{
    private List<secondaryPorts.Category> _categories;
    private CategoriesAdapter _categoriesAdapter;
    private secondaryPorts.Category _firstCategory;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _categories = CategoryFactory.Create();
        _categoriesAdapter = new CategoriesAdapter(MediatorMock.Object);
        _firstCategory = _categories.First().ToEntity();
    }

    [TestMethod]
    public async Task WhenGetCategoriesAsync_ThenShouldReturnCategories()
    {
        //Arrange
        var domainEntities = _categories.MapToDomain();
        MediatorMock.Setup(m => m.Send(It.IsAny<IGetCategoriesQuery>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(domainEntities));

        //Act
        var result = await _categoriesAdapter.GetAllAsync(CancellationToken.None);

        //Assert
        Assert.IsTrue(ModelAssertionsUtils<primaryPorts.CategoryModel>.AreListsEqual(result.Data, domainEntities.MapToPrimary()));
    }

    [TestMethod]
    public async Task GivenCategories_WhenInsertAsync_ThenShouldReturnIds()
    {
        //Arrange
        var categoriesIds = _categories.Select(l => l.Id.GetValueOrDefault()).ToList();

        MediatorMock.Setup(m => m.Send(It.IsAny<IAddCategoriesCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(categoriesIds));

        var upsertCategories = CategoryFactory.CreateUpsertModels();

        //Act
        var result = await _categoriesAdapter.InsertAsync(upsertCategories, CancellationToken.None);

        //Assert
        Assert.AreEqual(categoriesIds, result.Data);
    }

    [TestMethod]
    public async Task GivenCategoryAndId_WhenUpdateAsync_ThenShouldReturnId()
    {
        //Arrange
        var upsertCategory = CategoryFactory.CreateUpsertModel();

        MediatorMock.Setup(m => m.Send(It.IsAny<IUpdateCategoryCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(_firstCategory.Id.GetValueOrDefault()));

        //Act
        var result = await _categoriesAdapter.UpdateAsync(_firstCategory.Id.GetValueOrDefault(), upsertCategory, CancellationToken.None);

        //Assert
        Assert.AreEqual(_firstCategory.Id.GetValueOrDefault(), result.Data);
    }

    [TestMethod]
    public async Task GivenCategoryId_WhenDeleteAsync_ThenShouldReturnOk()
    {
        //Arrange

        MediatorMock.Setup(m => m.Send(It.IsAny<IDeleteCategoryCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok());

        //Act
        var result = await _categoriesAdapter.DeleteAsync(_firstCategory.Id.GetValueOrDefault(), CancellationToken.None);

        //Assert
        Assert.IsTrue(result.Success);
    }
}