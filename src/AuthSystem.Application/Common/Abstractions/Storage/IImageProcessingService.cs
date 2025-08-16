namespace AuthSystem.Application.Common.Abstractions.Storage;

/// <summary>
/// سرویس پردازش تصویر
/// </summary>
public interface IImageProcessingService
{
    Task<Stream> ResizeImageAsync(Stream imageStream, int width, int height, CancellationToken cancellationToken = default);
}
