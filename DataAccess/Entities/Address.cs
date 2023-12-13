using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Address
{
    public int Id { get; set; }

    public string? StreetName { get; set; }

    public int? StreetNumber { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
