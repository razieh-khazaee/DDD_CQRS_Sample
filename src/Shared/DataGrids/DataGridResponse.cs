using Microsoft.EntityFrameworkCore;

namespace Shared.DataGrids;

public class DataGridResponse<T>
{
    private DataGridResponse(List<T> items, int totalCount, bool hasNextPage, bool hasPreviousPage)
    {
        Items = items;
        TotalCount = totalCount;
        HasNextPage = hasNextPage;
        HasPreviousage = hasNextPage;
    }

    public List<T> Items { get; }

    public int TotalCount { get; }

    public bool HasNextPage { get; }

    public bool HasPreviousage { get; }

    public static async Task<DataGridResponse<T>> Create(IQueryable<T> query, int page, int pageSize)
    {
        if (page <= 0)
        {
            page = 1;
        }

        if (pageSize <= 0)
        {
            pageSize = 0;
        }

        var loadDataWithoutPaging = pageSize == 0;
        if (!loadDataWithoutPaging)
        {
            query = query
              .Skip((page - 1) * pageSize)
              .Take(pageSize);
        }

        var totalCount = await query.CountAsync();
        var items = await query.ToListAsync();
        var hasNextPage = loadDataWithoutPaging ? false : page * pageSize < totalCount;
        var hasPreviousPage = loadDataWithoutPaging ? false : page > 1;

        return new DataGridResponse<T>(items, totalCount, hasNextPage, hasPreviousPage);
    }
}