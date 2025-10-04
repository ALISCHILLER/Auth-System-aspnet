using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthSystem.Domain.Common.Policies;

/// <summary>
/// Represents the outcome of evaluating a policy.
/// </summary>
public sealed class PolicyResult
{
    private readonly List<string> _messages = new();

    private PolicyResult(bool isSatisfied, IEnumerable<string>? messages = null)
    {
        IsSatisfied = isSatisfied;
        if (messages is not null)
        {
            _messages.AddRange(messages.Where(message => !string.IsNullOrWhiteSpace(message)));
        }
    }
    public bool IsSatisfied { get; }
    public IReadOnlyCollection<string> Messages => _messages.AsReadOnly();

    public static PolicyResult Success(string? message = null) =>
        new(true, message is null ? null : new[] { message });


    public static PolicyResult Failure(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Failure message cannot be empty.", nameof(message));
        }

        return new(false, new[] { message });
    }

   
    public static PolicyResult Combine(IEnumerable<PolicyResult> results)
    {
        if (results is null) throw new ArgumentNullException(nameof(results));
        var array = results.ToArray();
        var isSatisfied = array.All(result => result.IsSatisfied);
        var messages = array.SelectMany(result => result._messages);
        return new PolicyResult(isSatisfied, messages);
    }
}