using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Api.Extensions;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Primary.Ports.OperationContracts.Adapters;

namespace OnlineShop.Api.Controllers;

[Authorize]
[Route("api/categories")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoriesAdapter _categoriesAdapter;

    public CategoriesController(ICategoriesAdapter categoriesAdapter)
        => _categoriesAdapter = categoriesAdapter;

    [HttpGet]
    public async Task<ActionResult> GetAsync(CancellationToken cancellationToken)
        => await _categoriesAdapter.GetAllAsync(cancellationToken)
            .ToAsyncActionResult();

    [HttpPost]
    public async Task<ActionResult> InsertAsync([FromBody] List<UpsertCategory> categories, CancellationToken cancellationToken)
        => await _categoriesAdapter.InsertAsync(categories, cancellationToken)
            .ToAsyncActionResult();

    [HttpPut]
    public async Task<ActionResult> UpdateAsync([FromQuery] Guid id,
        [FromBody] UpsertCategory upsertCategory, CancellationToken cancellationToken)
        => await _categoriesAdapter.UpdateAsync(id, upsertCategory, cancellationToken)
            .ToAsyncActionResult();

    [HttpDelete]
    public async Task<ActionResult> DeleteAsync([FromQuery] Guid id, CancellationToken cancellationToken)
        => await _categoriesAdapter.DeleteAsync(id, cancellationToken)
            .ToAsyncActionResult();
}