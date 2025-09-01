using CQRS.Data.DAL;
using CQRS.Data.Models;
using CQRS.DataAccess.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CQRS.Commands.Books;

public class CheckoutBookCommand : ICommand
{
    public ObjectId PatronId { get; set; }
    public ObjectId BookId { get; set; }
}

public class CheckoutBookHandler : ICommandHandler<CheckoutBookCommand>
{
    private readonly IMongoDb _context;
    private IMongoCollection<Book> BooksCollection { get; }
    private IMongoCollection<Patron> PatronsCollection { get; }

    public CheckoutBookHandler(IMongoDb context)
    {
        _context = context;
        BooksCollection = _context.Database.GetCollection<Book>("Books");
        PatronsCollection = _context.Database.GetCollection<Patron>("Patrons");
    }

    public async Task Handle(CheckoutBookCommand command, CancellationToken cancellationToken)
    {
        var patrons = await PatronsCollection.FindAsync<Patron>(x => x.Id.Equals(command.PatronId), default, cancellationToken);
        var books = await BooksCollection.FindAsync<Book>(x => x.Id.Equals(command.BookId), default, cancellationToken);

        var book = await books.FirstOrDefaultAsync(cancellationToken);
        if(book is null) throw new Exception("Book not found");
        var patron = await patrons.FirstOrDefaultAsync(cancellationToken);
        if(patron is null) throw new Exception("Patron not found");
        
        book.NumberAvailable = book.NumberAvailable - 1;
        book.CheckedOutTo.Add(patron.Id);
        
        patron.CheckedBooks.Add(new CheckedBooks()
        {
            Id = book.Id,
            Title = book.Title,
            CheckoutDate = DateTimeOffset.UtcNow
        });
        
        await BooksCollection.ReplaceOneAsync(x => x.Id.Equals(book.Id), book, cancellationToken: cancellationToken);
        await PatronsCollection.ReplaceOneAsync(x => x.Id.Equals(patron.Id), patron, cancellationToken: cancellationToken);
    }
}