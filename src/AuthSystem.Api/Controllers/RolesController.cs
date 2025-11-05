using System;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Contracts.Roles;
using AuthSystem.Application.Features.Roles.Commands.AssignRoleToUser;
using AuthSystem.Application.Features.Roles.Commands.CreateRole;
using AuthSystem.Application.Features.Roles.Queries.GetAllRoles;
using AuthSystem.Application.Roles.Commands.AssignRoleToUser;
using AuthSystem.Application.Roles.Commands.CreateRole.Contracts;
using AuthSystem.Application.Roles.Commands.CreateRole;
using AuthSystem.Application.Roles.Queries.GetAllRoles.Contracts;
using AuthSystem.Application.Roles.Queries.GetAllRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Api.Controllers;

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

    [HttpPost]
    [ProducesResponseType(typeof(CreateRoleResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetAll), new { version = "1.0" }, response);
    }

    [HttpPost("{roleId:guid}/assign/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Assign(Guid roleId, Guid userId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new AssignRoleToUserCommand(userId, roleId), cancellationToken).ConfigureAwait(false);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetAllRolesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAllRolesQuery(), cancellationToken).ConfigureAwait(false);
        return Ok(response);
    }
}