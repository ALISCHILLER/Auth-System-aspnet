namespace AuthSystem.Application.Interfaces;

/// <summary>
/// واسط برای مدیریت رمز عبور (هش و تأیید)
/// </summary>
public interface IPasswordService
{
    string Hash(string password);
    bool Verify(string password, string hash);
}