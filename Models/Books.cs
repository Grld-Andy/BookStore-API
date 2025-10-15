using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models;

[Table("Books")]
public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("title")]
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = null!;

    [Column("author")]
    [Required]
    [MaxLength(255)]
    public string Author { get; set; } = null!;

    [Column("yearPublished")]
    public int YearPublished { get; set; }
}