using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Api.Extensions;
using OnlineShop.Primary.Ports.OperationContracts.Adapters;

namespace OnlineShop.Api.Controllers;

[Authorize]
[Route("api/images")]
public class ImagesController : ControllerBase
{
    private readonly IImagesAdapter _imagesAdapter;

    public ImagesController(IImagesAdapter imagesAdapter)
        => _imagesAdapter = imagesAdapter;

    [HttpGet("{imageId}")]
    public async Task<IActionResult> Get(Guid imageId, CancellationToken cancellationToken)
        => await _imagesAdapter.Get(imageId, cancellationToken)
            .ToAsyncActionResult();

}