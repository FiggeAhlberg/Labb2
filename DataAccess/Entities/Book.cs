using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Book
{
    public string Isbn13 { get; set; } = null!;

    public string? Title { get; set; }

    public string? Language { get; set; }

    public int? Price { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public virtual ICollection<OrderBook> OrderBooks { get; set; } = new List<OrderBook>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Publisher> Publishers { get; set; } = new List<Publisher>();
}
