using AuthSystem.Domain.Common.Entities;
using AuthSystem.Domain.Exceptions;
using PhoneNumbers;
using System.Text.RegularExpressions;

namespace AuthSystem.Domain.ValueObjects;

/// <summary>
/// Value Object شماره تلفن با پشتیبانی بین‌المللی
/// </summary>
public sealed class PhoneNumber : ValueObject
{
    private static readonly PhoneNumberUtil PhoneUtil = PhoneNumberUtil.GetInstance();

    public string Value { get; }
    public string CountryCode { get; }
    public string NationalNumber { get; }
    public string Region { get; }

    private PhoneNumber(string value, string countryCode, string nationalNumber, string region)
    {
        Value = value;
        CountryCode = countryCode;
        NationalNumber = nationalNumber;
        Region = region;
    }

    public static PhoneNumber Create(string phoneNumber, string defaultRegion = "IR")
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw InvalidPhoneNumberException.ForEmptyPhoneNumber();

        try
        {
            phoneNumber = CleanPhoneNumber(phoneNumber);
            var parsedNumber = PhoneUtil.Parse(phoneNumber, defaultRegion);

            if (!PhoneUtil.IsValidNumber(parsedNumber))
                throw InvalidPhoneNumberException.ForInvalidNumber(phoneNumber);

            var e164Format = PhoneUtil.Format(parsedNumber, PhoneNumberFormat.E164);
            var countryCode = parsedNumber.CountryCode.ToString();
            var nationalNumber = parsedNumber.NationalNumber.ToString();
            var region = PhoneUtil.GetRegionCodeForNumber(parsedNumber);

            return new PhoneNumber(e164Format, countryCode, nationalNumber, region);
        }
        catch (NumberParseException ex)
        {
            throw InvalidPhoneNumberException.ForInvalidFormat(phoneNumber, "فرمت شماره تلفن نامعتبر است", ex);
        }
    }

    /// <summary>
    /// ایجاد شماره تلفن از فرمت E164
    /// </summary>
    public static PhoneNumber CreateFromE164(string e164Number)
    {
        if (string.IsNullOrWhiteSpace(e164Number))
            throw InvalidPhoneNumberException.ForEmptyPhoneNumber();

        if (!e164Number.StartsWith("+"))
            e164Number = "+" + e164Number;

        try
        {
            var parsedNumber = PhoneUtil.Parse(e164Number, null);

            if (!PhoneUtil.IsValidNumber(parsedNumber))
                throw InvalidPhoneNumberException.ForInvalidNumber(e164Number);

            var countryCode = parsedNumber.CountryCode.ToString();
            var nationalNumber = parsedNumber.NationalNumber.ToString();
            var region = PhoneUtil.GetRegionCodeForNumber(parsedNumber);

            return new PhoneNumber(e164Number, countryCode, nationalNumber, region);
        }
        catch (NumberParseException ex)
        {
            throw InvalidPhoneNumberException.ForInvalidFormat(e164Number, "فرمت E164 نامعتبر است", ex);
        }
    }

    /// <summary>
    /// فرمت‌دهی شماره به صورت بین‌المللی
    /// </summary>
    public string ToInternationalFormat()
    {
        try
        {
            var parsedNumber = PhoneUtil.Parse(Value, null);
            return PhoneUtil.Format(parsedNumber, PhoneNumberFormat.INTERNATIONAL);
        }
        catch
        {
            return Value;
        }
    }

    /// <summary>
    /// فرمت‌دهی شماره به صورت ملی
    /// </summary>
    public string ToNationalFormat()
    {
        try
        {
            var parsedNumber = PhoneUtil.Parse(Value, null);
            return PhoneUtil.Format(parsedNumber, PhoneNumberFormat.NATIONAL);
        }
        catch
        {
            return NationalNumber;
        }
    }

    private static string CleanPhoneNumber(string phoneNumber)
        => Regex.Replace(phoneNumber, @"[\s\-\(\)]", "");

    protected override IEnumerable<object> GetEqualityComponents() => new[] { Value };

    public override string ToString() => Value;

    /// <summary>
    /// اپراتور تبدیل ضمنی از PhoneNumber به string
    /// </summary>
    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;
}
