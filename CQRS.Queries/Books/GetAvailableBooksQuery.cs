using CQRS.Data.DAL;
using CQRS.Data.Models;
using CQRS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Queries.Books;

public class GetAvailableBooksQuery : IQuery<List<Book>>
{
}

public class GetAvailableBooksHandler : IQueryHandler<GetAvailableBooksQuery, List<Book>>
{
    private readonly ApplicationDbContext _context;

    public GetAvailableBooksHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> Handle(GetAvailableBooksQuery query, CancellationToken cancellationToken)
    {
        return await _context.Books.ToListAsync(cancellationToken);
    }
}