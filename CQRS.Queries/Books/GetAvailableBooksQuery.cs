using CQRS.Data.DAL;
using CQRS.Data.Models;
using CQRS.DataAccess.Interfaces;
using MongoDB.Driver;

namespace CQRS.Queries.Books;

public class GetAvailableBooksQuery : IQuery<List<Book>>
{
}

public class GetAvailableBooksHandler : IQueryHandler<GetAvailableBooksQuery, List<Book>>
{
    private readonly IMongoDb _context;
    private IMongoCollection<Book> Collection { get; }

    public GetAvailableBooksHandler(IMongoDb context)
    {
        _context = context;
        Collection = _context.Database.GetCollection<Book>("Books");
    }

    public async Task<List<Book>> Handle(GetAvailableBooksQuery query, CancellationToken cancellationToken)
    {
        var result = await Collection.FindAsync<Book>(x => true, default, cancellationToken);
        if (result is null) return new List<Book>();
        return await result.ToListAsync(cancellationToken);
    }
}