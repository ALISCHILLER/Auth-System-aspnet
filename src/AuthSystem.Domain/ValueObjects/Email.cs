using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AuthSystem.Domain.Common;

namespace AuthSystem.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        private const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        private static readonly Regex EmailRegex = new(EmailPattern, RegexOptions.Compiled);

        public string Value { get; }

        private Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), "آدرس ایمیل نمی‌تواند خالی باشد");

            value = value.Trim().ToLowerInvariant();

            if (!EmailRegex.IsMatch(value))
                throw new ArgumentException("فرمت ایمیل نامعتبر است", nameof(value));

            Value = value;
        }

        public static Email Create(string email) => new(email);

        public static bool TryCreate(string email, out Email? result)
        {
            try
            {
                result = Create(email);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static implicit operator string(Email email) => email.Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        // نمونه متدهای کمکی
        public string GetDomainPart()
        {
            return Value.Split('@').Last();
        }

        public string GetLocalPart()
        {
            return Value.Split('@').First();
        }

        public bool IsInDomain(string domain)
        {
            return GetDomainPart().Equals(domain, StringComparison.OrdinalIgnoreCase);
        }
    }
}