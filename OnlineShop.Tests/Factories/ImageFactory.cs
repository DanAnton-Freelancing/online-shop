using System;
using Amazon.S3.Model;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;
using System.IO;
using System.Net;
using System.Threading;

namespace OnlineShop.Tests.Factories;

public static class ImageFactory
{
    public static secondaryPorts.Image Create()
        =>
            new()
            {
                Key = "key",
            };

    public static secondaryPorts.Image ToEntity(this secondaryPorts.Image image)
    {
        image.Id = Guid.NewGuid();
        return image;
    }


    public static primaryPorts.Image MapToPrimary(this secondaryPorts.Image image, GetObjectResponse s3Object)
    {
        byte[] imageByteArray;
        using (var memoryStream = new MemoryStream())
        {
            s3Object.ResponseStream.CopyToAsync(memoryStream, CancellationToken.None);
            imageByteArray = memoryStream.ToArray();
        }

        return new primaryPorts.Image
        {
            Id = image.Id,
            File = imageByteArray
        };
    }

    public static GetObjectResponse S3Object()
    {
        var docStream = new FileInfo("./Docs/apple-image.jpg").OpenRead();
        return new GetObjectResponse
        {
            BucketName = "test",
            Key = "img",
            HttpStatusCode = HttpStatusCode.OK,
            ResponseStream = docStream,
        };
    }
}