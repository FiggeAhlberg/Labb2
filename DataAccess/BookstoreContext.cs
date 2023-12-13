using System;
using System.Collections.Generic;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public partial class BookstoreContext : DbContext
{
    public BookstoreContext()
    {
    }

    public BookstoreContext(DbContextOptions<BookstoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderBook> OrderBooks { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<VSalesStatistic> VSalesStatistics { get; set; }

    public virtual DbSet<VTitlePerAuthor> VTitlePerAuthors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-E9M1ELD;Initial Catalog=Bookstore;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Address__3214EC07CCEE5FE9");

            entity.ToTable("Address");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(30);
            entity.Property(e => e.Country).HasMaxLength(30);
            entity.Property(e => e.StreetName).HasMaxLength(100);
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Author__3214EC0772E6885A");

            entity.ToTable("Author");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(30);

            entity.HasMany(d => d.Books).WithMany(p => p.Authors)
                .UsingEntity<Dictionary<string, object>>(
                    "AuthorBook",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PK_BookId_Books"),
                    l => l.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PK_AuthorId_Authors"),
                    j =>
                    {
                        j.HasKey("AuthorId", "BookId").HasName("PK_AuthorBooks");
                        j.ToTable("Author_Books");
                        j.IndexerProperty<string>("BookId")
                            .HasMaxLength(13)
                            .IsUnicode(false)
                            .IsFixedLength();
                    });
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn13).HasName("PK__Books__3BF79E037E772E9B");

            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN13");
            entity.Property(e => e.Language).HasMaxLength(30);
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC077E53FAD2");

            entity.ToTable("Customer");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FirstName).HasMaxLength(30);
            entity.Property(e => e.LastName).HasMaxLength(30);

            entity.HasOne(d => d.Address).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__Customer__Addres__1AD3FDA4");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_OrderId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("PK_CustomerId_Customers");

            entity.HasOne(d => d.StoreNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Store)
                .HasConstraintName("PK_Store_Stores");
        });

        modelBuilder.Entity<OrderBook>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.BookId }).HasName("PK_OrderBooks");

            entity.ToTable("Order_Books");

            entity.Property(e => e.BookId)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Book).WithMany(p => p.OrderBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PK_Ord_BookId_Books");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderBooks)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PK_OrderId_Orders");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publishe__3214EC073C1CD561");

            entity.ToTable("Publisher");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasMany(d => d.Books).WithMany(p => p.Publishers)
                .UsingEntity<Dictionary<string, object>>(
                    "PublisherBook",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PK_Pub_BookId_Books"),
                    l => l.HasOne<Publisher>().WithMany()
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("PK_PublisherId_Publishers"),
                    j =>
                    {
                        j.HasKey("PublisherId", "BookId").HasName("PK_PublisherBooks");
                        j.ToTable("Publisher_Books");
                        j.IndexerProperty<string>("BookId")
                            .HasMaxLength(13)
                            .IsUnicode(false)
                            .IsFixedLength();
                    });
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.Isbn13 });

            entity.ToTable("Stock");

            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN13");

            entity.HasOne(d => d.Isbn13Navigation).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.Isbn13)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PK_ISBN13_Books");

            entity.HasOne(d => d.Store).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PK_StoreId_Stores");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stores__3214EC07B6B727FA");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.StoreName).HasMaxLength(100);

            entity.HasOne(d => d.Address).WithMany(p => p.Stores)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__Stores__AddressI__3B75D760");
        });

        modelBuilder.Entity<VSalesStatistic>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vSalesStatistics");

            entity.Property(e => e.CitiesSoldTo).HasMaxLength(30);
            entity.Property(e => e.StoreName).HasMaxLength(100);
        });

        modelBuilder.Entity<VTitlePerAuthor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vTitlePerAuthor");

            entity.Property(e => e.Name).HasMaxLength(51);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
