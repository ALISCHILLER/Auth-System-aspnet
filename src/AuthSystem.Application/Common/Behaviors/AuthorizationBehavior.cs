using AuthSystem.Application.Common.Abstractions.Authorization;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Application.Common.Exceptions;
using AuthSystem.Domain.Enums;
using MediatR;

namespace AuthSystem.Application.Common.Behaviors;

public sealed class AuthorizationBehavior<TRequest, TResponse>(
    ICurrentUserService currentUserService,
    IPermissionService permissionService,
    ICurrentUserPermissionCache permissionCache)
    : IPipelineBehavior<TRequest, TResponse>
       where TRequest : notnull, IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is IRequirePermission requirement)
        {
            var currentUserId = currentUserService.UserId;
            if (currentUserId is null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            if (!permissionCache.TryGet(currentUserId.Value, out var cachedPermissions))
            {
                var permissions = await permissionService
                    .GetPermissionsAsync(currentUserId.Value, cancellationToken)
                    .ConfigureAwait(false);

                cachedPermissions = permissions as IReadOnlySet<PermissionType> ?? new HashSet<PermissionType>(permissions);
                permissionCache.Set(currentUserId.Value, cachedPermissions);
            }

            if (!cachedPermissions.Contains(requirement.RequiredPermission))
            {
                throw new ForbiddenException($"Missing permission: {requirement.RequiredPermission}");
            }
        }

        return await next().ConfigureAwait(false);
    }
}