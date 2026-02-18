using CQRS.Data.DAL;
using CQRS.Data.Models;
using CQRS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Commands.Books;

public class CheckoutBookCommand : ICommand
{
    public int PatronId { get; set; }
    public int BookId { get; set; }
}

public class CheckoutBookHandler : ICommandHandler<CheckoutBookCommand>
{
    private readonly IDbContext _context;

    public CheckoutBookHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CheckoutBookCommand command, CancellationToken cancellationToken)
    {
        var book = await _context.Context.Books
            .FirstOrDefaultAsync(x => x.Id == command.BookId, cancellationToken);
        var patron = await _context.Context.Patrons
            .Include(p => p.CheckedBooks)
            .FirstOrDefaultAsync(x => x.Id == command.PatronId, cancellationToken);

        if(book is null) throw new Exception("Book not found");
        if(patron is null) throw new Exception("Patron not found");
        
        book.NumberAvailable = book.NumberAvailable - 1;
        book.CheckedOutTo.Add(patron.Id);
        
        patron.CheckedBooks.Add(new CheckedBooks()
        {
            Id = book.Id,
            Title = book.Title,
            CheckoutDate = DateTimeOffset.UtcNow
        });
        
        await _context.Context.SaveChangesAsync(cancellationToken);
    }
}