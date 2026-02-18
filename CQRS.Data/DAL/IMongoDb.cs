namespace CQRS.Data.DAL;

/// <summary>
/// IDbContext is an interface for database context. The primary purpose of this interface is for decoupling.
/// </summary>
public interface IDbContext
{
    /// <summary>
    /// Access to the ApplicationDbContext for database operations
    /// </summary>
    ApplicationDbContext Context { get; }
}