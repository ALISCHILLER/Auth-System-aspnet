using AuthSystem.Application.Common;
using AuthSystem.Application.Interfaces;
using AuthSystem.Domain.Entities;
using AuthSystem.Domain.Enums;
using MediatR;

namespace AuthSystem.Application.Features.User.ConfirmEmail;

/// <summary>
/// پردازش‌گر دستور تأیید ایمیل
/// </summary>
public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public ConfirmEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            return Result.Failure(AuthStatus.UserNotFound, "کاربر یافت نشد");

        if (user.EmailConfirmed)
            return Result.Failure(AuthStatus.EmailNotConfirmed, "ایمیل قبلاً تأیید شده است");

        // در عمل واقعی، باید توکن رو اعتبارسنجی کنید
        // مثلاً با استفاده از یک سرویس TokenService
        // اینجا فقط برای نمونه فرض می‌کنیم توکن معتبر است

        user.ConfirmEmail();
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result.Success(AuthStatus.Success, "ایمیل با موفقیت تأیید شد");
    }
}