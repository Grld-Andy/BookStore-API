// using Microsoft.EntityFrameworkCore;

// namespace BookStore.Data;

// public class BookStoreContext : DbContext
// {
//     public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }

//     public DbSet<Book> Books { get; set; } = null!;

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         base.OnModelCreating(modelBuilder);
//         modelBuilder.Entity<Book>().ToTable("Books");
//     }
// }

using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

public class BookStoreContext : DbContext
{
    public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }
    
    public DbSet<Book> Books { get; set; }
}