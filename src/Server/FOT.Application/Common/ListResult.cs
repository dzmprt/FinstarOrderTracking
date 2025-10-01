namespace FOT.Application.Common;

public class ListResult<TModel>
{
    public TModel[] Items { get; init; }

    public int TotalCount { get; init; }
}