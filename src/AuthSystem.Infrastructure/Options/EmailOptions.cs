namespace AuthSystem.Infrastructure.Options;

public sealed class EmailOptions
{

    public const string SectionName = "Email";

    public string Provider { get; set; } = "noop";

    public string From { get; set; } = "no-reply@example.com";
    public string? DisplayName { get; set; } = "Auth System";

    public string Host { get; set; } = "localhost";

    public int Port { get; set; } = 25;

    public bool EnableSsl { get; set; }
        = false;


    public string? Username { get; set; }
        = null;

    public string? Password { get; set; }
        = null;
}