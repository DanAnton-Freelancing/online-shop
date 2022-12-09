using Microsoft.AspNetCore.Http;
using OnlineShop.Secondary.Ports.OperationContracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Amazon.S3.Model;
using Amazon.S3;

namespace OnlineShop.Secondary.Adapters.Implementation;

public class S3StorageService : IS3StorageService
{
    private readonly IAmazonS3 _s3Client;

    public S3StorageService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<IList<string>> Add(string prefix, string bucketName, string folderName, IEnumerable<IFormFile> files, CancellationToken cancellationToken)
    {
        var imagesKeys = new List<string>();
        if (!DoesS3BucketExits(bucketName).Result)
        {
            await _s3Client.PutBucketAsync(bucketName, cancellationToken);
        }

        //if (entity?.Files == null)
        //    return await Mediator.Send(new AddProductsCommand { Data = entity.MapToSecondary(imagesKeys) },
        //        cancellationToken);

        foreach (var file in files)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix)
                    ? file.FileName
                    : $"{prefix.TrimEnd('/')}/{folderName}/{file.FileName}",
                InputStream = file.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", file.ContentType);
            await _s3Client.PutObjectAsync(request, cancellationToken);
            imagesKeys.Add(request.Key);
        }
        return imagesKeys;
    }

    private async Task<bool> DoesS3BucketExits(string bucketName)
        => await _s3Client.DoesS3BucketExistAsync(bucketName);
}