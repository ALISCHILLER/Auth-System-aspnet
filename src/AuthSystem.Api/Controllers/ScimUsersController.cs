using AuthSystem.Application.Common.Abstractions.Identity;
using AuthSystem.Application.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace AuthSystem.Api.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/scim/v2/Users")]
public sealed class ScimUsersController(IScimUserService scimUserService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(JsonObject), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(int startIndex = 1, int count = 50, CancellationToken cancellationToken = default)
    {
        var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";
        var users = await scimUserService.SearchAsync(startIndex, count, cancellationToken).ConfigureAwait(false);

        var response = new JsonObject
        {
            ["schemas"] = new JsonArray("urn:ietf:params:scim:api:messages:2.0:ListResponse"),
            ["totalResults"] = users.Count,
            ["startIndex"] = startIndex,
            ["itemsPerPage"] = users.Count,
            ["Resources"] = new JsonArray(users.Select(user => ConvertToResource(user, version)).ToArray())
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(JsonObject), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var user = await scimUserService.GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        if (user is null)
        {
            return NotFound();
        }

        var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";
        return Ok(ConvertToResource(user, version));
    }

    [HttpPost]
    [ProducesResponseType(typeof(JsonObject), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] ScimUserResource resource, CancellationToken cancellationToken)
    {
        var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";
        var user = await scimUserService.CreateAsync(resource, cancellationToken).ConfigureAwait(false);
        var payload = ConvertToResource(user, version);
        return CreatedAtAction(nameof(GetById), new { id = user.Id, version }, payload);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(JsonObject), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Replace(string id, [FromBody] ScimUserResource resource, CancellationToken cancellationToken)
    {
        var user = await scimUserService.ReplaceAsync(id, resource, cancellationToken).ConfigureAwait(false);
        if (user is null)
        {
            return NotFound();
        }

        var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";
        return Ok(ConvertToResource(user, version));
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(JsonObject), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Patch(string id, [FromBody] JsonObject request, CancellationToken cancellationToken)
    {
        var user = await scimUserService.PatchAsync(id, request, cancellationToken).ConfigureAwait(false);
        if (user is null)
        {
            return NotFound();
        }

        var version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "1.0";
        return Ok(ConvertToResource(user, version));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var deleted = await scimUserService.DeleteAsync(id, cancellationToken).ConfigureAwait(false);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    private static JsonObject ConvertToResource(ScimUserRepresentation user, string apiVersion)
    {
        var emails = new JsonArray();
        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            emails.Add(new JsonObject
            {
                ["value"] = user.Email,
                ["primary"] = true
            });
        }

        return new JsonObject
        {
            ["schemas"] = new JsonArray("urn:ietf:params:scim:schemas:core:2.0:User"),
            ["id"] = user.Id,
            ["userName"] = user.UserName,
            ["displayName"] = user.DisplayName,
            ["active"] = user.Active,
            ["emails"] = emails,
            ["meta"] = new JsonObject
            {
                ["resourceType"] = "User",
                ["created"] = user.CreatedAtUtc,
                ["location"] = $"/api/v{apiVersion}/scim/v2/Users/{user.Id}"
            }
        };
    }
}