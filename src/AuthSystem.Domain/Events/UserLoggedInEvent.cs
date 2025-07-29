using AuthSystem.Domain.Common;
using System;

namespace AuthSystem.Domain.Events;

/// <summary>
/// رویداد ورود کاربر به سیستم
/// این رویداد هنگام ورود موفق کاربر به سیستم ایجاد می‌شود
/// </summary>
public class UserLoggedInEvent : IDomainEvent
{
	/// <summary>
	/// شناسه کاربر
	/// </summary>
	public Guid UserId { get; }

	/// <summary>
	/// آدرس IP که ورود از آن انجام شده
	/// </summary>
	public string? IpAddress { get; }

	/// <summary>
	/// اطلاعات User Agent (مرورگر/دستگاه)
	/// </summary>
	public string? UserAgent { get; }

	/// <summary>
	/// زمان وقوع رویداد
	/// </summary>
	public DateTime OccurredOn { get; }

	/// <summary>
	/// سازنده رویداد
	/// </summary>
	/// <param name="userId">شناسه کاربر</param>
	/// <param name="ipAddress">آدرس IP</param>
	/// <param name="userAgent">اطلاعات User Agent</param>
	public UserLoggedInEvent(Guid userId, string? ipAddress, string? userAgent)
	{
		UserId = userId;
		IpAddress = ipAddress;
		UserAgent = userAgent;
		OccurredOn = DateTime.UtcNow;
	}
}