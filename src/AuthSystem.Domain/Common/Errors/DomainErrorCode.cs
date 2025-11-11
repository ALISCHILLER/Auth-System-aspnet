namespace AuthSystem.Domain.Common.Errors;

/// <summary>
/// Canonical error codes used across the domain layer and mapped to RFC 9457 problem types.
/// </summary>
public static class DomainErrorCode
{
    public const string Validation = "domain.validation";
    public const string Conflict = "domain.conflict";
    public const string NotFound = "domain.not_found";
    public const string Unauthorized = "domain.unauthorized";
    public const string Forbidden = "domain.forbidden";
    public const string RateLimited = "domain.rate_limited";

    private const string DefaultProblemTypeRoot = "https://problems.authsystem.dev";

    /// <summary>
    /// Maps an internal domain error code to an RFC 9457 compliant problem type URI.
    /// </summary>
    public static string ToProblemType(string errorCode)
    {
        if (string.IsNullOrWhiteSpace(errorCode))
        {
            return $"{DefaultProblemTypeRoot}/unknown";
        }

        var sanitized = errorCode.Trim().ToLowerInvariant().Replace(' ', '-');
        return $"{DefaultProblemTypeRoot}/{sanitized}";
    }

    /// <summary>
    /// Tries to parse a problem type URI back into a domain error code.
    /// </summary>
    public static string FromProblemType(string? problemType)
    {
        if (string.IsNullOrWhiteSpace(problemType))
        {
            return Validation;
        }

        if (problemType.StartsWith(DefaultProblemTypeRoot, StringComparison.OrdinalIgnoreCase))
        {
            return problemType[(DefaultProblemTypeRoot.Length + 1)..];
        }

        return problemType;
    }
}