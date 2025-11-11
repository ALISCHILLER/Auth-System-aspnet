namespace AuthSystem.Domain.Common.Rules;

/// <summary>
/// Represents the evaluation result of a business rule.
/// </summary>
public sealed class RuleResult
{
    private readonly List<string> _messages = new();

    private RuleResult(bool isSatisfied, IEnumerable<string>? messages = null)
    {
        IsSatisfied = isSatisfied;
        if (messages is not null)
        {
            _messages.AddRange(messages.Where(message => !string.IsNullOrWhiteSpace(message)));
        }
    }

    public bool IsSatisfied { get; }
    public IReadOnlyCollection<string> Messages => _messages.AsReadOnly();

    public static RuleResult Success(string? message = null) =>
        new(true, message is null ? null : new[] { message });

    public static RuleResult Failure(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Failure message cannot be empty.", nameof(message));
        }

        return new(false, new[] { message });
    }


    public static RuleResult Combine(IEnumerable<RuleResult> results)
    {
        if (results is null) throw new ArgumentNullException(nameof(results));
        var array = results.ToArray();
        var isSatisfied = array.All(result => result.IsSatisfied);
        var messages = array.SelectMany(result => result._messages);
        return new RuleResult(isSatisfied, messages);
    }
}