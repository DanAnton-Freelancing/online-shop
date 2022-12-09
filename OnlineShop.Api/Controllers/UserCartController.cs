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
[Route("api/userCart")]
public class UserCartController : ControllerBase
{
    private readonly IUserCartAdapter _userCartAdapter;

    public UserCartController(IUserCartAdapter userCartAdapter)
        => _userCartAdapter = userCartAdapter;

    [HttpGet]
    public async Task<ActionResult> GetAsync(Guid userId, CancellationToken cancellationToken)
        => await _userCartAdapter.GetWithDetailsAsync(userId, cancellationToken)
            .ToAsyncActionResult();

    [HttpPost]
    public async Task<ActionResult> AddItemAsync([FromBody] UpsertCartItemModel cartItemModel, CancellationToken cancellationToken)
        => await _userCartAdapter.AddItemAsync(cartItemModel, cartItemModel.UserId, cancellationToken)
            .ToAsyncActionResult();

    [HttpPut]
    public async Task<ActionResult> UpdateQuantity([FromQuery] Guid itemId,
        [FromBody] double quantity, CancellationToken cancellationToken)
        => await _userCartAdapter.UpdateItemQuantityAsync(itemId, quantity, cancellationToken)
            .ToAsyncActionResult();


    [HttpDelete]
    public async Task RemoveItemAsync([FromQuery] Guid itemId, CancellationToken cancellationToken)
        => await _userCartAdapter.RemoveItemAsync(itemId, cancellationToken)
            .ToAsyncActionResult();
}