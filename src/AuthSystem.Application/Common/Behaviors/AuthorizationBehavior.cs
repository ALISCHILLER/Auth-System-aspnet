using MediatR;
using AuthSystem.Application.Abstractions;
using AuthSystem.Domain.Enums;

namespace AuthSystem.Application.Common.Behaviors;

public interface IRequirePermission
{
    PermissionType RequiredPermission { get; }
}

public sealed class AuthorizationBehavior<TRequest, TResponse>(ICurrentUserService currentUserService)
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is IRequirePermission permissionAware)
        {
            if (currentUserService.UserId is null || !await currentUserService.HasPermissionAsync(permissionAware.RequiredPermission, cancellationToken))
            {
                throw new System.UnauthorizedAccessException("Insufficient permissions.");
            }
        }

        return await next();
    }
}