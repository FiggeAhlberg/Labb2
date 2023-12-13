using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Store
{
    public int Id { get; set; }

    public string? StoreName { get; set; }

    public int? AddressId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
