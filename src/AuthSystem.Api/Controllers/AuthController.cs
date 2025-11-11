using AuthSystem.Application.Contracts.Users;
using AuthSystem.Application.Features.Users.Commands.LoginUser;
using AuthSystem.Application.Features.Users.Commands.Logout;
using AuthSystem.Application.Features.Users.Commands.RefreshToken;
using AuthSystem.Application.Features.Users.Commands.TwoFactor.Request;
using AuthSystem.Application.Features.Users.Commands.TwoFactor.Verify;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AuthSystem.Api.Controllers;

/// <summary>
/// Exposes authentication endpoints for issuing and revoking tokens.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Issues an access and refresh token for a valid credential pair.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("auth-login")]
    [ProducesResponseType(typeof(ApiResponse<LoginUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
    {
        var enrichedCommand = command with
        {
            TenantId = ResolveTenantId(),
            IpAddress = ResolveIpAddress(),
            UserAgent = ResolveUserAgent(),
            ExternalLogin = ResolveExternalLogin()
        };

        var response = await _mediator.Send(enrichedCommand, cancellationToken).ConfigureAwait(false);
        return Ok(ApiResponse<LoginUserResponse>.Ok(response, HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Exchanges a valid refresh token for a new access token pair.
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [EnableRateLimiting("auth-refresh")]
    [ProducesResponseType(typeof(ApiResponse<LoginUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return Ok(ApiResponse<LoginUserResponse>.Ok(response, HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Issues a two-factor verification code via the preferred channel.
    /// </summary>
    [HttpPost("two-factor/request")]
    [Authorize]
    [EnableRateLimiting("two-factor-request")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> RequestTwoFactor([FromBody] RequestTwoFactorCodeCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        var response = ApiResponse<string>.Ok("Two-factor code dispatched.", HttpContext.TraceIdentifier);
        return Accepted(response);
    }

    /// <summary>
    /// Validates a previously issued two-factor verification code.
    /// </summary>
    [HttpPost("two-factor/verify")]
    [Authorize]
    [EnableRateLimiting("two-factor-verify")]
    [ProducesResponseType(typeof(ApiResponse<TwoFactorVerificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyTwoFactor([FromBody] VerifyTwoFactorCodeCommand command, CancellationToken cancellationToken)
    {
        var enrichedCommand = command with
        {
            TenantId = ResolveTenantId(),
            IpAddress = ResolveIpAddress(),
            UserAgent = ResolveUserAgent()
        };

        var response = await _mediator.Send(enrichedCommand, cancellationToken).ConfigureAwait(false);
        return Ok(ApiResponse<TwoFactorVerificationResponse>.Ok(response, HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Revokes the active refresh token for the current user.
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    [EnableRateLimiting("auth-logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command, CancellationToken cancellationToken)
    {
        var enrichedCommand = command with
        {
            TenantId = ResolveTenantId(),
            IpAddress = ResolveIpAddress(),
            UserAgent = ResolveUserAgent()
        };

        await _mediator.Send(enrichedCommand, cancellationToken).ConfigureAwait(false);
        return NoContent();
    }

    private string? ResolveTenantId()
        => Request.Headers.TryGetValue(TenantConstants.HeaderName, out var tenantId)
            ? tenantId.ToString()
            : User.FindFirst(TenantConstants.ClaimType)?.Value;

    private string? ResolveIpAddress()
        => HttpContext.Connection.RemoteIpAddress?.ToString();

    private string? ResolveUserAgent()
        => Request.Headers.UserAgent.ToString();

    private bool ResolveExternalLogin()
        => Request.Headers.TryGetValue("X-External-Login", out var external)
            && bool.TryParse(external, out var parsed)
            && parsed;
}