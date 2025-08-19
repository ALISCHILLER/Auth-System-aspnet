using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using AuthSystem.Domain.Common.Policies;
using AuthSystem.Domain.Services.Contracts;

namespace AuthSystem.Domain.Common.Mocks;

/// <summary>
/// شبیه‌سازی محدودکننده نرخ برای تست‌ها
/// </summary>
public class MockRateLimiter : IRateLimiter
{
    private readonly ConcurrentDictionary<string, int> _requestCounts = new();
    private readonly ConcurrentDictionary<string, DateTime> _lastRequestTimes = new();
    private readonly TimeSpan _window = TimeSpan.FromSeconds(1);
    private readonly int _maxRequests = 5;

    /// <summary>
    /// سازنده با تنظیمات پیش‌فرض
    /// </summary>
    public MockRateLimiter()
    {
    }

    /// <summary>
    /// سازنده با تنظیمات سفارشی
    /// </summary>
    public MockRateLimiter(int maxRequests, TimeSpan window)
    {
        _maxRequests = maxRequests;
        _window = window;
    }

    /// <summary>
    /// بررسی آیا درخواست مجاز است
    /// </summary>
    public RateLimitResult CheckLimit(string key)
    {
        var now = DateTime.UtcNow;
        var count = _requestCounts.AddOrUpdate(key, 1, (k, v) => v + 1);
        _lastRequestTimes[key] = now;

        var result = new RateLimitResult
        {
            IsAllowed = count <= _maxRequests,
            Remaining = Math.Max(0, _maxRequests - count),
            Reset = now.Add(_window)
        };

        // تمیز کردن درخواست‌های قدیمی
        CleanOldRequests(now);

        return result;
    }

    /// <summary>
    /// تمیز کردن درخواست‌های قدیمی
    /// </summary>
    private void CleanOldRequests(DateTime now)
    {
        var keysToRemove = _lastRequestTimes
            .Where(x => now - x.Value > _window)
            .Select(x => x.Key)
            .ToList();

        foreach (var key in keysToRemove)
        {
            _requestCounts.TryRemove(key, out _);
            _lastRequestTimes.TryRemove(key, out _);
        }
    }

    /// <summary>
    /// ریست کردن وضعیت شبیه‌سازی
    /// </summary>
    public void Reset()
    {
        _requestCounts.Clear();
        _lastRequestTimes.Clear();
    }

    /// <summary>
    /// شبیه‌سازی موفقیت‌آمیز بودن درخواست
    /// </summary>
    public void AllowRequest(string key)
    {
        _requestCounts[key] = 0;
    }

    /// <summary>
    /// شبیه‌سازی محدودیت درخواست
    /// </summary>
    public void LimitRequest(string key)
    {
        _requestCounts[key] = _maxRequests + 1;
    }
}