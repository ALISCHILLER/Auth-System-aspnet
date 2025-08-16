namespace AuthSystem.Application.Common.Abstractions.Security;

/// <summary>
/// سرویس هش و بررسی رمز عبور
/// </summary>
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string hash, string password);
}
