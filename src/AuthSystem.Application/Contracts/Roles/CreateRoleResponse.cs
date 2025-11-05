using System;

namespace AuthSystem.Application.Contracts.Roles;

public sealed class CreateRoleResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}