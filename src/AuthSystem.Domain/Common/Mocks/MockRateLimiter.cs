using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
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
    private bool _isEnabled = true;

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
            Reset = now.Add(_window),
            Total = _maxRequests
        };

        // تمیز کردن درخواست‌های قدیمی
        CleanOldRequests(now);

        return result;
    }

    /// <summary>
    /// بررسی آیا درخواست مجاز است با تنظیمات سفارشی
    /// </summary>
    public RateLimitResult CheckLimit(string key, int maxRequests, int window)
    {
        var now = DateTime.UtcNow;
        var count = _requestCounts.AddOrUpdate(key, 1, (k, v) => v + 1);
        _lastRequestTimes[key] = now;

        var result = new RateLimitResult
        {
            IsAllowed = count <= maxRequests,
            Remaining = Math.Max(0, maxRequests - count),
            Reset = now.Add(TimeSpan.FromSeconds(window)),
            Total = maxRequests
        };

        // تمیز کردن درخواست‌های قدیمی
        CleanOldRequests(now);

        return result;
    }

    /// <summary>
    /// ریست کردن محدودیت نرخ برای یک کلید
    /// </summary>
    public void ResetLimit(string key)
    {
        _requestCounts.TryRemove(key, out _);
        _lastRequestTimes.TryRemove(key, out _);
    }

    /// <summary>
    /// ریست کردن تمام محدودیت‌های نرخ
    /// </summary>
    public void ResetAllLimits()
    {
        _requestCounts.Clear();
        _lastRequestTimes.Clear();
    }

    /// <summary>
    /// دریافت وضعیت فعلی محدودیت نرخ برای یک کلید
    /// </summary>
    public RateLimitStatus GetStatus(string key)
    {
        _requestCounts.TryGetValue(key, out int count);
        _lastRequestTimes.TryGetValue(key, out DateTime lastRequestTime);

        var now = DateTime.UtcNow;
        var isLimited = count > _maxRequests;
        var resetTime = lastRequestTime.Add(_window);

        return new RateLimitStatus
        {
            Count = count,
            LastRequest = lastRequestTime,
            IsLimited = isLimited,
            ResetTime = resetTime
        };
    }

    /// <summary>
    /// آیا سرویس محدودکننده نرخ فعال است
    /// </summary>
    public bool IsEnabled()
    {
        return _isEnabled;
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

    /// <summary>
    /// فعال یا غیرفعال کردن سرویس
    /// </summary>
    public void SetEnabled(bool isEnabled)
    {
        _isEnabled = isEnabled;
    }
}