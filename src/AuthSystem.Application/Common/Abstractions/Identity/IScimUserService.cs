using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using AuthSystem.Application.Contracts.Users;

namespace AuthSystem.Application.Common.Abstractions.Identity;

public interface IScimUserService
{
    Task<IReadOnlyCollection<ScimUserRepresentation>> SearchAsync(int startIndex, int count, CancellationToken cancellationToken);
    Task<ScimUserRepresentation?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<ScimUserRepresentation> CreateAsync(ScimUserResource resource, CancellationToken cancellationToken);
    Task<ScimUserRepresentation?> ReplaceAsync(string id, ScimUserResource resource, CancellationToken cancellationToken);
    Task<ScimUserRepresentation?> PatchAsync(string id, JsonObject patchRequest, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}