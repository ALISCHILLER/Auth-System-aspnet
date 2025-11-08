using System;
using System.Collections.Generic;

namespace AuthSystem.Shared.DTOs;

public sealed class PagedRequest
{
    private const int MaxPageSize = 100;

    public PagedRequest(int pageNumber = 1, int pageSize = 10)
    {
        PageNumber = Math.Max(1, pageNumber);
        PageSize = Math.Clamp(pageSize, 1, MaxPageSize);
    }

    public int PageNumber { get; }

    public int PageSize { get; }

    public IDictionary<string, string> Filters { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
}