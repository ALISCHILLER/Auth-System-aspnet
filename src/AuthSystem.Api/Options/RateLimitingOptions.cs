namespace AuthSystem.Api.Options;

public sealed class RateLimitingOptions
{
    public const string SectionName = "RateLimiting";

    public FixedWindowPolicyOptions Global { get; set; } = new() { PermitLimit = 120, WindowSeconds = 60, QueueLimit = 0 };

    public FixedWindowPolicyOptions AuthLogin { get; set; } = new() { PermitLimit = 10, WindowSeconds = 60, QueueLimit = 2 };

    public FixedWindowPolicyOptions AuthRefresh { get; set; } = new() { PermitLimit = 30, WindowSeconds = 60, QueueLimit = 5 };

    public FixedWindowPolicyOptions AuthLogout { get; set; } = new() { PermitLimit = 60, WindowSeconds = 60, QueueLimit = 5 };

    public FixedWindowPolicyOptions TwoFactorRequest { get; set; } = new() { PermitLimit = 5, WindowSeconds = 300, QueueLimit = 1 };

    public FixedWindowPolicyOptions TwoFactorVerify { get; set; } = new() { PermitLimit = 10, WindowSeconds = 300, QueueLimit = 2 };

    public sealed class FixedWindowPolicyOptions
    {
        public int PermitLimit { get; set; } = 60;
        public int WindowSeconds { get; set; } = 60;
        public int QueueLimit { get; set; } = 0;
    }
}