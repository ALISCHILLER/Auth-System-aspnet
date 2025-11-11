namespace AuthSystem.Domain.Common.Extensions;

/// <summary>
/// اکستنشن‌های برای مجموعه‌ها
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// آیا مجموعه خالی است
    /// </summary>
    public static bool IsEmpty<T>(this IEnumerable<T> source)
    {
        return source == null || !source.Any();
    }

    /// <summary>
    /// آیا مجموعه خالی نیست
    /// </summary>
    public static bool IsNotEmpty<T>(this IEnumerable<T> source)
    {
        return source != null && source.Any();
    }

    /// <summary>
    /// انتخاب تصادفی یک آیتم
    /// </summary>
    public static T Random<T>(this IEnumerable<T> source)
    {
        var list = source.ToList();
        if (list.IsEmpty())
            throw new InvalidOperationException("Collection is empty");

        return list[new Random().Next(list.Count)];
    }

    /// <summary>
    /// تقسیم مجموعه به صفحات
    /// </summary>
    public static IEnumerable<IEnumerable<T>> Paginate<T>(this IEnumerable<T> source, int pageSize)
    {
        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than zero", nameof(pageSize));

        var list = source.ToList();
        for (var i = 0; i < list.Count; i += pageSize)
        {
            yield return list.Skip(i).Take(pageSize);
        }
    }

    /// <summary>
    /// تبدیل به لیست با اطمینان از عدم خالی بودن
    /// </summary>
    public static List<T> ToNonEmptyList<T>(this IEnumerable<T> source)
    {
        var list = source.ToList();
        if (list.IsEmpty())
            throw new InvalidOperationException("Collection cannot be empty");

        return list;
    }

    /// <summary>
    /// گروه‌بندی بر اساس چندین فیلد
    /// </summary>
    public static IEnumerable<IGrouping<string, TElement>> GroupByMultiple<TElement>(
        this IEnumerable<TElement> source,
        params Func<TElement, object>[] keySelectors)
    {
        return source.GroupBy(item =>
            string.Join("|", keySelectors
                .Select(selector => selector(item)?.ToString() ?? string.Empty)));
    }

    /// <summary>
    /// یافتن اولین مورد یا ایجاد استثنا
    /// </summary>
    public static T FirstOrThrow<T>(this IEnumerable<T> source, string errorMessage = null)
    {
        var item = source.FirstOrDefault();
        if (item == null)
            throw new InvalidOperationException(errorMessage ?? "Item not found");

        return item;
    }
}