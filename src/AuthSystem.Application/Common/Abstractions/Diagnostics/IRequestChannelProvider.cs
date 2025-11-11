namespace AuthSystem.Application.Common.Abstractions.Diagnostics;

public interface IRequestChannelProvider
{
    string Channel { get; }
}