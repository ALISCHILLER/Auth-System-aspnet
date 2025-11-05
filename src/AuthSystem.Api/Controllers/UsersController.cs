using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthSystem.Application.Contracts.Users;
using AuthSystem.Application.Features.Users.Commands.RegisterUser;
using AuthSystem.Application.Features.Users.Queries.GetUserById;
using AuthSystem.Application.Users.Commands.RegisterUser.Contracts;
using AuthSystem.Application.Users.Commands.RegisterUser;
using AuthSystem.Application.Users.Queries.GetUserById.Contracts;
using AuthSystem.Application.Users.Queries.GetUserById;

namespace AuthSystem.Api.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetById), new { version = "1.0", userId = response.Id }, response);
    }

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(GetUserByIdResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid userId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserByIdQuery(userId), cancellationToken).ConfigureAwait(false);
        return Ok(response);
    }
}