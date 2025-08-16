namespace AuthSystem.Application.Common.Primitives;

/// <summary>
/// یک کتابخانه متمرکز برای خطاهای رایج در سیستم.
/// از این کلاس استاتیک برای جلوگیری از Magic Strings و تضمین ثبات استفاده کنید.
/// </summary>
public static class Errors
{
    public static class General
    {
        public static Error Unhandled() =>
            Error.Failure("یک خطای پیش‌بینی نشده رخ داده است.", "General.Unhandled");

        public static Error NotFound(Guid? id = null) =>
            id.HasValue
                ? Error.NotFound($"موجودیت با شناسه '{id}' یافت نشد.", $"General.NotFound.{id}")
                : Error.NotFound("موجودیت مورد نظر یافت نشد.", "General.NotFound");
    }

    public static class Users
    {
        public static Error InvalidCredentials() =>
            Error.Unauthorized("نام کاربری یا رمز عبور نامعتبر است.", "Users.InvalidCredentials");

        public static Error EmailAlreadyInUse(string email) =>
            Error.Conflict($"ایمیل '{email}' قبلاً ثبت شده است.", "Users.EmailAlreadyInUse")
                .WithMetadata("email", email);

        public static Error NotFound(Guid userId) =>
            Error.NotFound($"کاربر با شناسه '{userId}' یافت نشد.", $"Users.NotFound.{userId}")
                .WithMetadata("userId", userId);
    }

    public static class Validation
    {
        public static Error IsRequired(string fieldName) =>
            Error.Validation($"فیلد '{fieldName}' اجباری است.", fieldName, "Validation.FieldIsRequired")
                .WithMetadata("field", fieldName);

        public static Error HasMaxLength(string fieldName, int maxLength) =>
            Error.Validation($"طول فیلد '{fieldName}' نباید بیشتر از {maxLength} کاراکتر باشد.", fieldName, "Validation.FieldMaxLength")
                .WithMetadata("field", fieldName)
                .WithMetadata("maxLength", maxLength);

        public static Error InvalidFormat(string fieldName) =>
            Error.Validation($"فرمت فیلد '{fieldName}' نامعتبر است.", fieldName, "Validation.FieldInvalidFormat")
                .WithMetadata("field", fieldName);
    }
}