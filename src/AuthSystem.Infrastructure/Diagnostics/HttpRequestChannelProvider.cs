using AuthSystem.Application.Common.Abstractions.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace AuthSystem.Infrastructure.Diagnostics;

internal sealed class HttpRequestChannelProvider(IHttpContextAccessor httpContextAccessor) : IRequestChannelProvider
{
    public string Channel
    {
        get
        {
            var context = httpContextAccessor.HttpContext;
            if (context is null)
            {
                return "Background";
            }

            if (context.WebSockets.IsWebSocketRequest)
            {
                return "SignalR";
            }

            var contentType = context.Request.ContentType;
            if (!string.IsNullOrWhiteSpace(contentType) && contentType.Contains("application/grpc", StringComparison.OrdinalIgnoreCase))
            {
                return "gRPC";
            }

            if (string.Equals(context.Request.Path, "/graphql", StringComparison.OrdinalIgnoreCase))
            {
                return "GraphQL";
            }

            return "HTTP";
        }
    }
}