using System;
using System.Collections.Generic;
using System.Linq;
using AuthSystem.Domain.Enums;
using AuthSystem.Domain.Exceptions;
using AuthSystem.Domain.ValueObjects;
using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Common.Rules;
using AuthSystem.Domain.Common.Policies;
using AuthSystem.Domain.Entities.UserAggregate;
using AuthSystem.Domain.Entities.RoleAggregate;
using AuthSystem.Domain.Entities.Authorization.Role;

namespace AuthSystem.Domain.Factories;

/// <summary>
/// Factory برای ایجاد کاربر
/// این کلاس مسئول ایجاد و تهیه کاربران با قوانین دامنه است
/// </summary>
public static class UserFactory
{
    /// <summary>
    /// ایجاد کاربر جدید با ایمیل
    /// </summary>
    public static User CreateUserWithEmail(
        string email,
        string password,
        string firstName,
        string lastName,
        string? phoneNumber = null,
        DateTime? dateOfBirth = null,
        string? nationalCode = null,
        bool isEmailVerified = false)
    {
        // اعتبارسنجی ایمیل
        var emailVo = Email.Create(email);

        // اعتبارسنجی رمز عبور
        var passwordHash = SecurityFactory.CreateSecurePassword(password);

        // اعتبارسنجی نام
        ValidateName(firstName, lastName);

        // اعتبارسنجی شماره تلفن (در صورت وجود)
        PhoneNumber? phoneVo = null;
        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            phoneVo = PhoneNumber.Create(phoneNumber);
        }

        // اعتبارسنجی کد ملی (در صورت وجود)
        NationalCode? nationalCodeVo = null;
        if (!string.IsNullOrWhiteSpace(nationalCode))
        {
            nationalCodeVo = NationalCode.Create(nationalCode);
        }

        // ایجاد کاربر
        return new User(
            Guid.NewGuid(),
            emailVo,
            passwordHash,
            firstName,
            lastName,
            phoneVo,
            dateOfBirth,
            nationalCodeVo,
            isEmailVerified
        );
    }

    /// <summary>
    /// ایجاد کاربر جدید با شماره تلفن
    /// </summary>
    public static User CreateUserWithPhone(
        string phoneNumber,
        string password,
        string firstName,
        string lastName,
        string? email = null,
        DateTime? dateOfBirth = null,
        string? nationalCode = null,
        bool isPhoneVerified = false)
    {
        // اعتبارسنجی شماره تلفن
        var phoneVo = PhoneNumber.Create(phoneNumber);

        // اعتبارسنجی رمز عبور
        var passwordHash = SecurityFactory.CreateSecurePassword(password);

        // اعتبارسنجی نام
        ValidateName(firstName, lastName);

        // اعتبارسنجی ایمیل (در صورت وجود)
        Email? emailVo = null;
        if (!string.IsNullOrWhiteSpace(email))
        {
            emailVo = Email.Create(email);
        }

        // اعتبارسنجی کد ملی (در صورت وجود)
        NationalCode? nationalCodeVo = null;
        if (!string.IsNullOrWhiteSpace(nationalCode))
        {
            nationalCodeVo = NationalCode.Create(nationalCode);
        }

        // ایجاد کاربر
        return new User(
            Guid.NewGuid(),
            emailVo,
            passwordHash,
            firstName,
            lastName,
            phoneVo,
            dateOfBirth,
            nationalCodeVo,
            isPhoneVerified: isPhoneVerified
        );
    }

    /// <summary>
    /// ایجاد کاربر جدید برای شبکه‌های اجتماعی
    /// </summary>
    public static User CreateUserForSocialLogin(
        string provider,
        string providerUserId,
        string email,
        string firstName,
        string lastName,
        string? phoneNumber = null)
    {
        if (string.IsNullOrWhiteSpace(provider))
            throw new ArgumentException("نام ارائه‌دهنده نمی‌تواند خالی باشد", nameof(provider));

        if (string.IsNullOrWhiteSpace(providerUserId))
            throw new ArgumentException("شناسه کاربر در شبکه اجتماعی نمی‌تواند خالی باشد", nameof(providerUserId));

        // اعتبارسنجی ایمیل
        var emailVo = Email.Create(email);

        // ایجاد رمز عبور موقت ایمن
        var passwordHash = SecurityFactory.CreateTemporaryPassword();

        // اعتبارسنجی نام
        ValidateName(firstName, lastName);

        // اعتبارسنجی شماره تلفن (در صورت وجود)
        PhoneNumber? phoneVo = null;
        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            phoneVo = PhoneNumber.Create(phoneNumber);
        }

        // ایجاد کاربر
        var user = new User(
            Guid.NewGuid(),
            emailVo,
            passwordHash,
            firstName,
            lastName,
            phoneVo,
            isEmailVerified: true,
            isSocialLogin: true
        );

        // افزودن اطلاعات شبکه اجتماعی
        user.AddSocialLogin(provider, providerUserId);

        return user;
    }

    /// <summary>
    /// اعتبارسنجی نام و نام خانوادگی
    /// </summary>
    private static void ValidateName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("نام نمی‌تواند خالی باشد", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("نام خانوادگی نمی‌تواند خالی باشد", nameof(lastName));

        // بررسی طول نام
        if (firstName.Length > 50)
            throw new ArgumentException("نام نمی‌تواند بیشتر از 50 کاراکتر باشد", nameof(firstName));

        if (lastName.Length > 100)
            throw new ArgumentException("نام خانوادگی نمی‌تواند بیشتر از 100 کاراکتر باشد", nameof(lastName));

        // بررسی کاراکترهای مجاز
        if (!System.Text.RegularExpressions.Regex.IsMatch(firstName, @"^[\p{L}\s'-]+$"))
            throw new ArgumentException("نام فقط می‌تواند شامل حروف، فاصله، خط تیره و آپاستروف باشد", nameof(firstName));

        if (!System.Text.RegularExpressions.Regex.IsMatch(lastName, @"^[\p{L}\s'-]+$"))
            throw new ArgumentException("نام خانوادگی فقط می‌تواند شامل حروف، فاصله، خط تیره و آپاستروف باشد", nameof(lastName));
    }

    /// <summary>
    /// افزودن نقش به کاربر
    /// </summary>
    public static void AddRoleToUser(User user, Role role)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        // بررسی آیا کاربر قبلاً این نقش را دارد
        if (user.Roles.Any(r => r.RoleId == role.Id))
            throw new InvalidUserRoleException(user.Id, role.Name, $"کاربر قبلاً نقش '{role.Name}' را دارد");

        // افزودن نقش
        user.AddRole(role.Id, role.Name);
    }

    /// <summary>
    /// افزودن چندین نقش به کاربر
    /// </summary>
    public static void AddRolesToUser(User user, IEnumerable<Role> roles)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (roles == null)
            throw new ArgumentNullException(nameof(roles));

        var roleList = roles.ToList();
        if (!roleList.Any())
            return;

        // بررسی تکراری بودن نقش‌ها
        var existingRoleIds = user.Roles.Select(r => r.RoleId).ToList();
        var newRoles = roleList
            .Where(r => !existingRoleIds.Contains(r.Id))
            .ToList();

        if (!newRoles.Any())
            return;

        // افزودن نقش‌ها
        foreach (var role in newRoles)
        {
            user.AddRole(role.Id, role.Name);
        }
    }

    /// <summary>
    /// حذف نقش از کاربر
    /// </summary>
    public static void RemoveRoleFromUser(User user, Role role)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (role == null)
            throw new ArgumentNullException(nameof(role));

        // بررسی آیا کاربر این نقش را دارد
        if (!user.Roles.Any(r => r.RoleId == role.Id))
            throw new InvalidUserRoleException(user.Id, role.Name, $"کاربر نقش '{role.Name}' را ندارد");

        // حذف نقش
        user.RemoveRole(role.Id);
    }

    /// <summary>
    /// ایجاد کاربر موقت برای تأیید ایمیل
    /// </summary>
    public static User CreateTemporaryUserForEmailVerification(
        string email,
        string firstName,
        string lastName,
        string password)
    {
        // ایجاد کاربر با وضعیت در انتظار تأیید
        var user = CreateUserWithEmail(
            email,
            password,
            firstName,
            lastName,
            isEmailVerified: false
        );

        // تنظیم وضعیت کاربر به در انتظار
        user.SetStatus(UserStatus.Pending);

        return user;
    }

    /// <summary>
    /// ایجاد کاربر موقت برای تأیید شماره تلفن
    /// </summary>
    public static User CreateTemporaryUserForPhoneVerification(
        string phoneNumber,
        string firstName,
        string lastName,
        string password)
    {
        // ایجاد کاربر با وضعیت در انتظار تأیید
        var user = CreateUserWithPhone(
            phoneNumber,
            password,
            firstName,
            lastName,
            isPhoneVerified: false
        );

        // تنظیم وضعیت کاربر به در انتظار
        user.SetStatus(UserStatus.Pending);

        return user;
    }

    /// <summary>
    /// ایجاد کاربر ادمین سیستم
    /// </summary>
    public static User CreateAdminUser(
        string email,
        string password,
        string firstName,
        string lastName,
        string? phoneNumber = null)
    {
        // ایجاد کاربر معمولی
        var admin = CreateUserWithEmail(
            email,
            password,
            firstName,
            lastName,
            phoneNumber
        );

        // تنظیم وضعیت کاربر به فعال
        admin.SetStatus(UserStatus.Active);

        // افزودن نقش ادمین
        admin.AddRole(Guid.Parse("A0000000-0000-0000-0000-000000000001"), "Admin");

        return admin;
    }

    /// <summary>
    /// ایجاد کاربر مهمان
    /// </summary>
    public static User CreateGuestUser()
    {
        // ایجاد ایمیل و رمز عبور موقت
        var email = $"guest-{Guid.NewGuid()}@example.com";
        var password = Guid.NewGuid().ToString();

        // ایجاد کاربر مهمان
        var guest = CreateUserWithEmail(
            email,
            password,
            "Guest",
            "User"
        );

        // تنظیم وضعیت کاربر به فعال
        guest.SetStatus(UserStatus.Active);

        // افزودن نقش مهمان
        guest.AddRole(Guid.Parse("G0000000-0000-0000-0000-000000000001"), "Guest");

        return guest;
    }

    /// <summary>
    /// ایجاد کاربر از اطلاعات احراز هویت دو عاملی
    /// </summary>
    public static User CreateUserWithTwoFactor(
        string email,
        string password,
        string firstName,
        string lastName,
        string? phoneNumber = null,
        bool isTwoFactorEnabled = false)
    {
        // ایجاد کاربر
        var user = CreateUserWithEmail(
            email,
            password,
            firstName,
            lastName,
            phoneNumber
        );

        // فعال‌سازی احراز هویت دو عاملی (در صورت درخواست)
        if (isTwoFactorEnabled)
        {
            var secretKey = SecurityFactory.CreateTwoFactorSecretKey();
            user.EnableTwoFactorAuthentication(secretKey);
        }

        return user;
    }

    /// <summary>
    /// ایجاد کاربر از اطلاعات لاگین موفق
    /// </summary>
    public static User CreateUserFromSuccessfulLogin(
        Guid userId,
        string email,
        string firstName,
        string lastName,
        DateTime loginTime,
        string ipAddress,
        string userAgent)
    {
        // اعتبارسنجی ایمیل
        var emailVo = Email.Create(email);

        // ایجاد کاربر
        var user = new User(
            userId,
            emailVo,
            null, // رمز عبور در این مرحله نیاز نیست
            firstName,
            lastName
        );

        // افزودن اطلاعات ورود
        user.RecordSuccessfulLogin(loginTime, ipAddress, userAgent);

        return user;
    }

    /// <summary>
    /// ایجاد کاربر از اطلاعات ورود ناموفق
    /// </summary>
    public static User CreateUserFromFailedLogin(
        Guid userId,
        string email,
        string firstName,
        string lastName,
        DateTime loginTime,
        string ipAddress,
        string userAgent,
        string failureReason)
    {
        // اعتبارسنجی ایمیل
        var emailVo = Email.Create(email);

        // ایجاد کاربر
        var user = new User(
            userId,
            emailVo,
            null, // رمز عبور در این مرحله نیاز نیست
            firstName,
            lastName
        );

        // افزودن اطلاعات ورود ناموفق
        user.RecordFailedLogin(loginTime, ipAddress, userAgent, failureReason);

        return user;
    }

    /// <summary>
    /// ایجاد کاربر از اطلاعات حساب کاربری قفل شده
    /// </summary>
    public static User CreateUserFromLockedAccount(
        Guid userId,
        string email,
        string firstName,
        string lastName,
        DateTime lockoutTime,
        string lockoutReason,
        int failedAttempts)
    {
        // اعتبارسنجی ایمیل
        var emailVo = Email.Create(email);

        // ایجاد کاربر
        var user = new User(
            userId,
            emailVo,
            null, // رمز عبور در این مرحله نیاز نیست
            firstName,
            lastName
        );

        // قفل کردن حساب
        user.LockAccount(lockoutTime, lockoutReason, failedAttempts);

        return user;
    }

    /// <summary>
    /// ایجاد کاربر از اطلاعات بازیابی رمز عبور
    /// </summary>
    public static User CreateUserFromPasswordReset(
        Guid userId,
        string email,
        string firstName,
        string lastName,
        DateTime resetTime,
        string resetToken)
    {
        // اعتبارسنجی ایمیل
        var emailVo = Email.Create(email);

        // ایجاد کاربر
        var user = new User(
            userId,
            emailVo,
            null, // رمز عبور در این مرحله نیاز نیست
            firstName,
            lastName
        );

        // افزودن اطلاعات بازیابی رمز عبور
        user.RecordPasswordResetRequest(resetTime, resetToken);

        return user;
    }

    /// <summary>
    /// ایجاد کاربر از اطلاعات ویرایش پروفایل
    /// </summary>
    public static User CreateUserFromProfileUpdate(
        Guid userId,
        string email,
        string firstName,
        string lastName,
        string? phoneNumber = null,
        DateTime? dateOfBirth = null,
        string? nationalCode = null)
    {
        // اعتبارسنجی ایمیل
        var emailVo = Email.Create(email);

        // اعتبارسنجی شماره تلفن (در صورت وجود)
        PhoneNumber? phoneVo = null;
        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            phoneVo = PhoneNumber.Create(phoneNumber);
        }

        // اعتبارسنجی کد ملی (در صورت وجود)
        NationalCode? nationalCodeVo = null;
        if (!string.IsNullOrWhiteSpace(nationalCode))
        {
            nationalCodeVo = NationalCode.Create(nationalCode);
        }

        // ایجاد کاربر
        var user = new User(
            userId,
            emailVo,
            null, // رمز عبور در این مرحله نیاز نیست
            firstName,
            lastName,
            phoneVo,
            dateOfBirth,
            nationalCodeVo
        );

        return user;
    }

    /// <summary>
    /// ایجاد کاربر از اطلاعات تغییر رمز عبور
    /// </summary>
    public static User CreateUserFromPasswordChange(
        Guid userId,
        string email,
        string firstName,
        string lastName,
        string newPassword)
    {
        // اعتبارسنجی ایمیل
        var emailVo = Email.Create(email);

        // اعتبارسنجی رمز عبور جدید
        var passwordHash = SecurityFactory.CreateSecurePassword(newPassword);

        // ایجاد کاربر
        var user = new User(
            userId,
            emailVo,
            passwordHash,
            firstName,
            lastName
        );

        // افزودن اطلاعات تغییر رمز عبور
        user.ChangePassword(passwordHash);

        return user;
    }
}