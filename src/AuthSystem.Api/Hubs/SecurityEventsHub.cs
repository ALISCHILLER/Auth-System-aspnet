using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AuthSystem.Api.Hubs;

/// <summary>
/// SignalR hub that emits security events in real-time for connected administrators.
/// </summary>
[Authorize]
public sealed class SecurityEventsHub : Hub
{
}