namespace CQRS.Data.DAL;

/// <summary>
/// DbContextWrapper is a concrete class and implements IDbContext. 
/// This provides access to the Entity Framework DbContext.
/// </summary>
public class DbContextWrapper : IDbContext
{
    /// <inheritdoc/>
    public ApplicationDbContext Context { get; }

    /// <summary>
    /// A DbContextWrapper instance contains the ApplicationDbContext for database operations.
    /// </summary>
    public DbContextWrapper(ApplicationDbContext context)
    {
        Context = context;
    }
}