using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class Author
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
