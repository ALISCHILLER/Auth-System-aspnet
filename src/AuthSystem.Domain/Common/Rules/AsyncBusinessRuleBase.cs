using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Rules;

/// <summary>
/// Base implementation for asynchronous business rules.
/// </summary>
public abstract class AsyncBusinessRuleBase : IAsyncBusinessRule
{
   
    public abstract string Message { get; }

    
    public virtual string ErrorCode => GetType().Name;

   
    public abstract Task<bool> IsBrokenAsync();


    public virtual bool IsBroken() => IsBrokenAsync().GetAwaiter().GetResult();

    public virtual Task<string> GetMessageAsync() => Task.FromResult(Message);
}