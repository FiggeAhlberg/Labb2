using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class VSalesStatistic
{
    public string? StoreName { get; set; }

    public int? TotalBooksSold { get; set; }

    public string? CitiesSoldTo { get; set; }
}
