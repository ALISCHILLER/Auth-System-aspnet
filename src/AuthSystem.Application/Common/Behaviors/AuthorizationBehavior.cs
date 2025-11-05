using System;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Application.Common.Abstractions.Authorization;
using AuthSystem.Application.Common.Abstractions.Security;
using AuthSystem.Application.Common.Exceptions;
using MediatR;

namespace AuthSystem.Application.Common.Behaviors;

public sealed class AuthorizationBehavior<TRequest, TResponse>(
    ICurrentUserService currentUserService,
    IPermissionService permissionService)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
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

            var permissions = await permissionService
                .GetPermissionsAsync(currentUserId.Value, cancellationToken)
                .ConfigureAwait(false);

            if (!permissions.Contains(requirement.RequiredPermission))
            {
                throw new ForbiddenException($"Missing permission: {requirement.RequiredPermission}");
            }
        }

        return await next().ConfigureAwait(false);
    }
}