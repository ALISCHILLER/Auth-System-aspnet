using AuthSystem.Domain.Common.Abstractions;

namespace AuthSystem.Domain.Common.Exceptions;


public class BusinessRuleValidationException : DomainException
{
    private BusinessRuleValidationException(string message, string errorCode)
      : base(message)
    {
        RuleMessage = message;
        ErrorCode = errorCode;
    }


    private BusinessRuleValidationException(string message, string errorCode, Exception innerException)
        : base(message, innerException)
    {
        RuleMessage = message;
        ErrorCode = errorCode;
    }

    public string RuleMessage { get; }

    public override string ErrorCode { get; }

    public static BusinessRuleValidationException ForBrokenRule(IBusinessRule rule)
    {
        return new BusinessRuleValidationException(rule.Message, rule.Code);
    }


    public static async Task<BusinessRuleValidationException> ForBrokenRuleAsync(IAsyncBusinessRule rule)
    {
        var message = await rule.GetMessageAsync().ConfigureAwait(false);
        return new BusinessRuleValidationException(message, rule.Code);
    }
}