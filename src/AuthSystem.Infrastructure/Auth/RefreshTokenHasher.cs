using System.Security.Cryptography;
using System.Text;

namespace AuthSystem.Infrastructure.Auth;

internal sealed class RefreshTokenHasher
{
    public string Hash(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes);
    }
}