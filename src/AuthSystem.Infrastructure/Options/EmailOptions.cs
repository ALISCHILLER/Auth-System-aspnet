namespace AuthSystem.Infrastructure.Options;

public sealed class EmailOptions
{
    public string From { get; set; } = "no-reply@example.com";
    public string? DisplayName { get; set; } = "Auth System";
    public string SmtpHost { get; set; } = "localhost";
    public int SmtpPort { get; set; } = 25;
    public bool UseSsl { get; set; } = false;
    public string? Username { get; set; }
        = null;
    public string? Password { get; set; }
        = null;
}