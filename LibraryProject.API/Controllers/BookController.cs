using Application.Commands.Books;
using Application.Models;
using Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers;

[ApiController]
[Route("/api/books")]
public class BookController : ControllerBase
{
    private readonly IMediator _mediator;
    public BookController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResultViewModel<BookViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post([FromBody] AddBookCommand book)
    {
        var result = await _mediator.Send(book);

        if (result.IsSuccess)
            return Ok(result.Data);
        
        return BadRequest(result.Message);
    }
    
}