using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Domain.Implementations.Queries;
using OnlineShop.Secondary.Ports.DataContracts;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Tests.Extensions;
using OnlineShop.Tests.Factories;

namespace OnlineShop.Tests.Domain.Queries;

[TestClass]
public class GetImageByIdQueryTests : BaseQueryTests
{
    private Image _image;
    private GetImageByIdQuery.GetImageByIdQueryHandler _getImageByIdQueryHandler;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _image = ImageFactory.Create().ToEntity();
        _getImageByIdQueryHandler = new GetImageByIdQuery.GetImageByIdQueryHandler(ReaderRepositoryMock.Object);

    }

    [TestMethod]
    public async Task WhenGetProducts_ThenShouldReturnProducts()
    {
        //Arrange
        var imageQuery = new GetImageByIdQuery
        {
            ImageId = _image.Id.GetValueOrDefault()
        };

        ReaderRepositoryMock.Setup(uc => uc.GetOneAsync(It.IsAny<Expression<Func<Image, bool>>>(),
                CancellationToken.None, It.IsAny<Func<IQueryable<Image>, IOrderedQueryable<Image>>>(),
                It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
            .ReturnsAsync(Result.Ok(_image));

        //Act
        var result = await _getImageByIdQueryHandler.Handle(imageQuery, CancellationToken.None);

        //Assert
        Assert.IsTrue(EntitiesAssertionsUtils<Image>.AreEntriesEqual(result.Data, _image));
        Assert.AreEqual(imageQuery.ImageId, _image.Id.GetValueOrDefault());

    }
}