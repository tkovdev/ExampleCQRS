using CQRS.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CQRS.Data.DAL;

/// <summary>
/// ApplicationDbContext is the Entity Framework Core database context for the CQRS application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Patron> Patrons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Book entity
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            // Store CheckedOutTo as JSON or create a separate many-to-many table
            entity.Property(e => e.CheckedOutTo)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList())
                .Metadata.SetValueComparer(new ValueComparer<IList<int>>(
                    (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
        });

        // Configure Patron entity
        modelBuilder.Entity<Patron>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            // Store CheckedBooks as owned entity collection
            entity.OwnsMany(p => p.CheckedBooks, cb =>
            {
                cb.WithOwner().HasForeignKey("PatronId");
                cb.Property<int>("Id").ValueGeneratedOnAdd();
                cb.HasKey("Id");
            });
        });
    }
}
