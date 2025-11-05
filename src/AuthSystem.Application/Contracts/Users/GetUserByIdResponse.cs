using System;

namespace AuthSystem.Application.Contracts.Users;

public sealed class GetUserByIdResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}