using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class OrderBook
{
    public int OrderId { get; set; }

    public string BookId { get; set; } = null!;

    public int? NumberOf { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
