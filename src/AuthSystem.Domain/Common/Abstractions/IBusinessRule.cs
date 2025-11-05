namespace AuthSystem.Domain.Common.Abstractions;

public interface IBusinessRule
{
    string Code { get; }
    string Message { get; }
    bool IsBroken();
}