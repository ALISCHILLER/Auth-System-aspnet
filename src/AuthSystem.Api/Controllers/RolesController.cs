using AuthSystem.Application.Contracts.Roles;
using AuthSystem.Application.Features.Roles.Commands.AssignRoleToUser;
using AuthSystem.Application.Features.Roles.Commands.CreateRole;
using AuthSystem.Application.Features.Roles.Queries.GetAllRoles;
using AuthSystem.Shared.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Api.Controllers;


/// <summary>
/// Handles role management scenarios including creation and assignment.
/// </summary>
[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/roles")]
public sealed class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new role within the system.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CreateRoleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        var envelope = ApiResponse<CreateRoleResponse>.Created(response, HttpContext.TraceIdentifier);
        return CreatedAtAction(nameof(GetAll), new { version = "1.0" }, envelope);
    }

    /// <summary>
    /// Assigns an existing role to a user.
    /// </summary>
    [HttpPost("{roleId:guid}/assign/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Assign(Guid roleId, Guid userId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AssignRoleToUserCommand(userId, roleId), cancellationToken).ConfigureAwait(false);
        return NoContent();
    }

    /// <summary>
    /// Retrieves all defined roles.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetAllRolesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllRolesQuery(), cancellationToken).ConfigureAwait(false);
        return Ok(ApiResponse<GetAllRolesResponse>.Ok(response, HttpContext.TraceIdentifier));
    }
}