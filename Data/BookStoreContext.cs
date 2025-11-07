using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

public class BookStoreContext : DbContext
{
    public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }
    
    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                Title = "The Pragmatic Programmer",
                Author = "Andrew Hunt and David Thomas",
                YearPublished = 1999
            },
            new Book
            {
                Id = 2,
                Title = "Clean Code",
                Author = "Robert C. Martin",
                YearPublished = 2008
            },
            new Book
            {
                Id = 3,
                Title = "Design Patterns: Elements of Reusable Object-Oriented Software",
                Author = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides",
                YearPublished = 1994
            },
            new Book
            {
                Id = 4,
                Title = "Refactoring: Improving the Design of Existing Code",
                Author = "Martin Fowler",
                YearPublished = 1999
            },
            new Book
            {
                Id = 5,
                Title = "Domain-Driven Design: Tackling Complexity in the Heart of Software",
                Author = "Eric Evans",
                YearPublished = 2003
            }
        );
    }
}