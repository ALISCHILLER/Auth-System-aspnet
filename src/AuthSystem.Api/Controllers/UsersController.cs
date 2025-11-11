using AuthSystem.Application.Contracts.Users;
using AuthSystem.Application.Features.Users.Commands.RegisterUser;
using AuthSystem.Application.Features.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Api.Controllers;


/// <summary>
/// Manages user-centric operations such as registration and profile retrieval.
/// </summary>
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

    /// <summary>
    /// Registers a new user and returns the created resource.
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<RegisterUserResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        var envelope = ApiResponse<RegisterUserResponse>.Created(response, HttpContext.TraceIdentifier);
        return CreatedAtAction(
            nameof(GetById),
            new { version = "1.0", userId = response.Id },
            envelope);
    }

    /// <summary>
    /// Fetches a user by the unique identifier.
    /// </summary>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetUserByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid userId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserByIdQuery(userId), cancellationToken).ConfigureAwait(false);
        return Ok(ApiResponse<GetUserByIdResponse>.Ok(response, HttpContext.TraceIdentifier));
    }
}