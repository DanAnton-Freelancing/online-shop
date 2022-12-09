using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Application.Implementations.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Queries;

[TestClass]
public class GetImageByIdQueryTests : BaseQueryTests
{
    private ImageDb _imageDb;
    private GetImageByIdQuery.GetImageByIdQueryHandler _getImageByIdQueryHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _imageDb = ImageFactory.Create().ToEntity();
        _getImageByIdQueryHandler = new GetImageByIdQuery.GetImageByIdQueryHandler(ReaderRepositoryMock.Object);

    }

    [TestMethod]
    public async Task WhenGetProducts_ThenShouldReturnProducts()
    {
        //Arrange
        var imageQuery = new GetImageByIdQuery
        {
            ImageId = _imageDb.Id.GetValueOrDefault()
        };

        ReaderRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<ImageDb, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<ImageDb>, IOrderedQueryable<ImageDb>>>(),
                It.IsAny<Func<IQueryable<ImageDb>, IIncludableQueryable<ImageDb, object>>>()))
            .ReturnsAsync(Result.Ok(_imageDb));

        //Act
        var result = await _getImageByIdQueryHandler.Handle(imageQuery, CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<ImageDb>.AreEntriesEqual(result.Data, _imageDb));
        Assert.AreEqual(imageQuery.ImageId, _imageDb.Id.GetValueOrDefault());

    }
}