using AuthSystem.Domain.Common;
using AuthSystem.Domain.ValueObjects;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Aggregates.UserAggregate.Rules;

/// <summary>
/// قانون: آدرس ایمیل باید منحصر به فرد باشد
/// </summary>
public class UserEmailMustBeUniqueRule : IAsyncBusinessRule
{
    private readonly Email _email;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// سازنده قانون
    /// </summary>
    public UserEmailMustBeUniqueRule(Email email, IUserRepository userRepository)
    {
        _email = email;
        _userRepository = userRepository;
    }

    /// <summary>
    /// پیام خطا در صورت نقض قانون
    /// </summary>
    public string Message => "این آدرس ایمیل قبلاً ثبت شده است";

    /// <summary>
    /// کد خطا در صورت نقض قانون
    /// </summary>
    public string ErrorCode => "EMAIL_EXISTS";

    /// <summary>
    /// بررسی نقض قانون (به صورت آسنکرون)
    /// </summary>
    public async Task<bool> IsBrokenAsync()
    {
        var existingUser = await _userRepository.GetByEmailAsync(_email);
        return existingUser != null;
    }
}