﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Api.Extensions;
using OnlineShop.Primary.Ports.DataContracts;
using OnlineShop.Primary.Ports.OperationContracts.Adapters;

namespace OnlineShop.Api.Controllers;

[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUsersAdapter _usersAdapter;

    public UsersController(IUsersAdapter usersAdapter)
        => _usersAdapter = usersAdapter;
        
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> LoginAsync([FromBody] LoginUserModel userModel, CancellationToken cancellationToken)
        => await _usersAdapter.LoginAsync(userModel, cancellationToken)
            .ToAsyncActionResult();

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Register([FromBody] RegisterUserModel userModel, CancellationToken cancellationToken)
        => await _usersAdapter.RegisterAsync(userModel, cancellationToken)
            .ToAsyncActionResult();
}