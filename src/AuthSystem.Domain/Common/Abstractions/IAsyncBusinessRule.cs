using AuthSystem.Domain.Common.Rules;
using System.Threading.Tasks;

namespace AuthSystem.Domain.Common.Abstractions;

public interface IAsyncBusinessRule : IBusinessRule
{
    Task<bool> IsBrokenAsync();
    Task<string> GetMessageAsync();
}