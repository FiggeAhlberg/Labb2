using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly? OrderDate { get; set; }

    public int? CustomerId { get; set; }

    public int? Store { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();

    public virtual Store? StoreNavigation { get; set; }
}
