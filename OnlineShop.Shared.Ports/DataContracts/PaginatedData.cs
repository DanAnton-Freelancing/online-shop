using System.Collections.Generic;

namespace OnlineShop.Shared.Ports.DataContracts;

public class PaginatedData<T>
{
    public List<T> Data { get; set; }
    public short Offset { get; set; }
    public short Limit { get; set; }
    public int Total { get; set; }
}