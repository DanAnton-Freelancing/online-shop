using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using OnlineShop.Application.Implementations.Commands.Products;
using OnlineShop.Application.Implementations.Queries;
using OnlineShop.Primary.Adapters.Mappers;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Primary.Ports.OperationContracts.Adapters;
using OnlineShop.Shared.Ports.DataContracts;
using OnlineShop.Shared.Ports.Extensions;

namespace OnlineShop.Primary.Adapters.Implementation;

public class ProductsAdapter : CrudAdapter<ProductModel, UpsertProductModel>, IProductsAdapter
{
    private readonly IAmazonS3 _s3Client;
    private const string BucketName = "ad-online-shop";
    private const string Prefix = "products";

    public ProductsAdapter(IMediator mediator, IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
        Mediator = mediator;
    }

    public override async Task<Result<List<ProductModel>>> GetAllAsync(CancellationToken cancellationToken)
        => await Mediator.Send(new GetProductsQuery(), cancellationToken)
            .MapAsync(p => p.MapToPrimary());

    public override Task<Result<List<Guid>>> InsertAsync(List<UpsertProductModel> entities, CancellationToken cancellationToken)
    {
        // return empty values, on purpose
        return Task.FromResult(Result.Ok(new List<Guid>()));
    }

    public async Task<Result<Guid>> InsertAsync(UpsertProductModel entity, CancellationToken cancellationToken)
    {
        var imagesKeys = new List<string>();
        if (entity?.Files == null)
            return await Mediator.Send(new AddProductsCommand { Data = entity.MapToSecondary(imagesKeys) },
                cancellationToken);

        imagesKeys = await AddToS3(entity.Name, entity.Files, cancellationToken);

        return await Mediator.Send(new AddProductsCommand { Data = entity.MapToSecondary(imagesKeys) }, cancellationToken);
    }

    private async Task<List<string>> AddToS3(string folderName, IEnumerable<IFormFile> files, CancellationToken cancellationToken)
    {
        var imagesKeys = new List<string>();
        if (!DoesS3BucketExits().Result)
        {
            await _s3Client.PutBucketAsync(BucketName, cancellationToken);
        }

        foreach (var file in files)
        {
            var request = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = string.IsNullOrEmpty(Prefix)
                    ? file.FileName
                    : $"{Prefix.TrimEnd('/')}/{folderName}/{file.FileName}",
                InputStream = file.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", file.ContentType);
            await _s3Client.PutObjectAsync(request, cancellationToken);
            imagesKeys.Add(request.Key);
        }
        return imagesKeys;
    }

    public override async Task<Result<Guid>> UpdateAsync(Guid id, UpsertProductModel entity, CancellationToken cancellationToken)
        => await Mediator.Send(new UpdateProductCommand { Id = id, Data = entity.MapToSecondary() }, cancellationToken);

    public override async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
        => await Mediator.Send(new DeleteProductCommand { Id = id }, cancellationToken);

    public async Task<Result<ProductModel>> GetById(Guid id, CancellationToken cancellationToken)
        => await Mediator.Send(new GetProductByIdQuery { Id = id }, cancellationToken)
            .MapAsync(p => p.MapToPrimary());

    private async Task<bool> DoesS3BucketExits()
        => await _s3Client.DoesS3BucketExistAsync(BucketName);
}