using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Api.Extensions;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Primary.Ports.OperationContracts.Adapters;

namespace OnlineShop.Api.Controllers;

[Authorize]
[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductsAdapter _productsAdapter;

    public ProductsController(IProductsAdapter productsAdapter)
        => _productsAdapter = productsAdapter;

    [HttpGet]
    public async Task<ActionResult> GetAsync(CancellationToken cancellationToken)
        => await _productsAdapter.GetAllAsync(cancellationToken)
            .ToAsyncActionResult();

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
        => await _productsAdapter.GetById(id, cancellationToken)
            .ToAsyncActionResult();

    [HttpPost]
    public async Task<ActionResult> InsertAsync([FromForm] UpsertProduct product, CancellationToken cancellationToken)
        => await _productsAdapter.InsertAsync(product, cancellationToken)
            .ToAsyncActionResult();

    [HttpPut]
    public async Task<ActionResult> UpdateAsync([FromQuery] Guid id,
        [FromForm] UpsertProduct upsertProduct, CancellationToken cancellationToken)
        => await _productsAdapter.UpdateAsync(id, upsertProduct, cancellationToken)
            .ToAsyncActionResult();

    [HttpDelete]
    public async Task<ActionResult> DeleteAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        => await _productsAdapter.DeleteAsync(id, cancellationToken)
            .ToAsyncActionResult();
}