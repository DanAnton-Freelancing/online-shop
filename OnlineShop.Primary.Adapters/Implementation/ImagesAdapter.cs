using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using MediatR;
using OnlineShop.Application.Implementations.Queries;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Primary.Ports.OperationContracts.Adapters;
using OnlineShop.Shared.Ports.DataContracts;

namespace OnlineShop.Primary.Adapters.Implementation;

public class ImagesAdapter : IImagesAdapter
{
    private const string BucketName = "ad-online-shop";
    private readonly IMediator _mediator;
    private readonly IAmazonS3 _s3Client;

    public ImagesAdapter(IMediator mediator, IAmazonS3 s3Client)
    {
        _mediator = mediator;
        _s3Client = s3Client;
    }

    public async Task<Result<ImageModel>> Get(Guid imageId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetImageByIdQuery { ImageId = imageId }, cancellationToken);

        var bucketExists = await _s3Client.DoesS3BucketExistAsync(BucketName);
        if (!bucketExists)
            return Result.Ok(new ImageModel());

        var s3Object = await _s3Client.GetObjectAsync(BucketName, result.Data.Key, cancellationToken);
        byte[] imageByteArray;
        using (var memoryStream = new MemoryStream())
        {
            await s3Object.ResponseStream.CopyToAsync(memoryStream, cancellationToken);
            imageByteArray = memoryStream.ToArray();
        }

        return Result.Ok(new ImageModel
        {
            Id = result.Data.Id,
            File = imageByteArray
        });
    }
}