﻿using System;
using Amazon.S3.Model;
using secondaryPorts = OnlineShop.Secondary.Ports.DataContracts;
using primaryPorts = OnlineShop.Primary.Ports.DataContracts;
using System.IO;
using System.Net;
using System.Threading;

namespace OnlineShop.Tests.Factories;

public static class ImageFactory
{
    public static secondaryPorts.ImageDb Create()
        =>
            new()
            {
                Key = "key",
            };

    public static secondaryPorts.ImageDb ToEntity(this secondaryPorts.ImageDb imageDb)
    {
        imageDb.Id = Guid.NewGuid();
        return imageDb;
    }


    public static primaryPorts.ImageModel MapToPrimary(this secondaryPorts.ImageDb imageDb, GetObjectResponse s3Object)
    {
        byte[] imageByteArray;
        using (var memoryStream = new MemoryStream())
        {
            s3Object.ResponseStream.CopyToAsync(memoryStream, CancellationToken.None);
            imageByteArray = memoryStream.ToArray();
        }

        return new primaryPorts.ImageModel
        {
            Id = imageDb.Id,
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