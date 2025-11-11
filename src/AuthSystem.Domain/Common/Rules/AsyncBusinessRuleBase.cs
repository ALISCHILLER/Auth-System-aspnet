using AuthSystem.Domain.Common.Abstractions;

namespace AuthSystem.Domain.Common.Rules;


public abstract class AsyncBusinessRuleBase : IAsyncBusinessRule
{

    public abstract string Message { get; }


    public virtual string Code => GetType().Name;


    public abstract Task<bool> IsBrokenAsync();


    public virtual bool IsBroken() => IsBrokenAsync().GetAwaiter().GetResult();

    public virtual Task<string> GetMessageAsync() => Task.FromResult(Message);
}