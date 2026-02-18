using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CQRS.Data.Models;

public class Patron
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IList<CheckedBooks> CheckedBooks { get; set; } = new List<CheckedBooks>();
}

public class CheckedBooks
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset CheckoutDate { get; set; }
    public DateTimeOffset? ReturnDate { get; set; }
}