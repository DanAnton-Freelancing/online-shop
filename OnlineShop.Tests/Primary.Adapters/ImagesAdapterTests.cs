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
using Amazon.S3;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Commands.Products;
using OnlineShop.Primary.Ports.OperationContracts.CQRS.Queries;

namespace OnlineShop.Tests.Primary.Adapters;

[TestClass]
public class ImagesAdapterTests : BaseTests
{
    private secondaryPorts.ImageDb _imageDb;
    private ImagesAdapter _imagesAdapter;
    private Mock<IAmazonS3> _s3ClientMock;

    [TestInitialize]
    public override void Initialize()
    {
        base.Initialize();
        _s3ClientMock = new Mock<IAmazonS3>();
        _imageDb = ImageFactory.Create();
        _imagesAdapter = new ImagesAdapter(MediatorMock.Object, _s3ClientMock.Object);
    }

    [TestMethod]
    public async Task WhenGetByIdAsync_ThenShouldReturnImage()
    {
        //Arrange
        var s3Image = ImageFactory.S3Object();

        MediatorMock.Setup(m => m.Send(It.IsAny<IGetImageByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(_imageDb));

        _s3ClientMock.Setup(m => m.DoesS3BucketExistAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _s3ClientMock.Setup(m => m.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(s3Image);

        //Act
        var result = await _imagesAdapter.Get(_imageDb.Id.GetValueOrDefault(), CancellationToken.None);

        //Assert
        Assert.IsTrue(ModelAssertionsUtils<primaryPorts.ImageModel>.AreEntriesEqual(result.Data, _imageDb.MapToPrimary(s3Image)));
    }

    [TestMethod]
    public async Task WhenGetByIdAsyncAndBucketDoesNotExist_ThenShouldReturnEmptyImage()
    {
        //Arrange
        var s3Image = ImageFactory.S3Object();

        MediatorMock.Setup(m => m.Send(It.IsAny<IGetImageByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(_imageDb));

        _s3ClientMock.Setup(m => m.DoesS3BucketExistAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _s3ClientMock.Setup(m => m.GetObjectAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(s3Image);

        //Act
        var result = await _imagesAdapter.Get(_imageDb.Id.GetValueOrDefault(), CancellationToken.None);

        //Assert
        Assert.IsTrue(ModelAssertionsUtils<primaryPorts.ImageModel>.AreEntriesEqual(result.Data, new primaryPorts.ImageModel()));
    }
}