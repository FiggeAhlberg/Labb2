using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class VTitlePerAuthor
{
    public string? Name { get; set; }

    public int? Age { get; set; }

    public int? NumberOfTitles { get; set; }

    public int? TotalStockValue { get; set; }
}
