using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Stock
{
    public int StoreId { get; set; }

    public string Isbn13 { get; set; } = null!;

    public int? Balance { get; set; }

    public virtual Book Isbn13Navigation { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
