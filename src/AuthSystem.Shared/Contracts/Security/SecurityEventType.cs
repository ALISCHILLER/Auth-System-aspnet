namespace AuthSystem.Shared.Contracts.Security;

/// <summary>
/// Represents the type of security event emitted by the platform.
/// </summary>
public enum SecurityEventType
{
    Login = 1,
    Logout = 2,
    TwoFactorFailed = 3
}