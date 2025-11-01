using System.Threading;
using System.Threading.Tasks;

namespace AuthSystem.Application.Abstractions;

public interface ISmsSender
{
    Task SendAsync(string phoneNumber, string message, CancellationToken ct = default);
}