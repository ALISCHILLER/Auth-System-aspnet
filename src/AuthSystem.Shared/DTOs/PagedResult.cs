using System;
using System.Collections.Generic;

namespace AuthSystem.Shared.DTOs;

public sealed class PagedResult<T>
{
    public PagedResult(IReadOnlyCollection<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items ?? Array.Empty<T>();
        PageNumber = Math.Max(1, pageNumber);
        PageSize = Math.Max(1, pageSize);
        TotalCount = Math.Max(0, totalCount);
    }

    public IReadOnlyCollection<T> Items { get; }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalCount { get; }

    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
}