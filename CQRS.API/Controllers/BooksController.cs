using CQRS.Commands.Books;
using CQRS.DataAccess.Interfaces;
using CQRS.Queries.Books;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly ILogger<BooksController> _logger;
    private readonly IMediator _mediator;

    public BooksController(ILogger<BooksController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableBooks()
    {
        var result = await _mediator.Send(new GetAvailableBooksQuery());
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost("checkout/{patronId}/{bookId}")]
    public async Task<IActionResult> CheckoutBook([FromRoute] int patronId, [FromRoute] int bookId)
    {
        await _mediator.Send(new CheckoutBookCommand(){BookId = bookId, PatronId = patronId});
        return StatusCode(StatusCodes.Status200OK);
    }
}