namespace FOT.Application.Common;

public class ListResult<TModel>(TModel[] items, int totalCount)
{
    public TModel[] Items { get; private set; } = items;

    public int TotalCount { get; private set; } = totalCount;
}
