namespace AuthSystem.Application.Common.Abstractions.Storage;

/// <summary>
/// سرویس ذخیره‌سازی فایل
/// </summary>
public interface IFileStorageService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
}
