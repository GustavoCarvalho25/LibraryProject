using Application.Commands.Books;
using Application.Models;
using Application.Queries.Books;
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
    
    [HttpGet]
    [ProducesResponseType(typeof(ResultViewModel<IEnumerable<BookViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllBooksQuery();
        var result = await _mediator.Send(query);
        
        if (result.IsSuccess)
            return Ok(result);
            
        return BadRequest(result.Message);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ResultViewModel<BookViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetBookByIdQuery(id);
        var result = await _mediator.Send(query);
        
        if (!result.IsSuccess)
            return NotFound(result.Message);
            
        return Ok(result);
    }
    
    [HttpGet("author/{authorName}")]
    [ProducesResponseType(typeof(ResultViewModel<IEnumerable<BookViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByAuthor(string authorName)
    {
        var query = new GetBooksByAuthorQuery(authorName);
        var result = await _mediator.Send(query);
        
        if (result.IsSuccess)
            return Ok(result);
            
        return BadRequest(result.Message);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResultViewModel<BookViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] AddBookCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result.Message);
        
        return Ok(result);
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ResultViewModel<BookViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateBookCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID in URL does not match ID in request body");
            
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
            return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
            
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new RemoveBookCommand(id);
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
            return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
            
        return Ok(result);
    }
    
    [HttpPost("{id:guid}/loan")]
    [ProducesResponseType(typeof(ResultViewModel<LoanViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LoanBook(Guid id, [FromBody] Guid userId)
    {
        var command = new LoanBookCommand(id, userId);
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
            return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
            
        return Ok(result);
    }
    
    [HttpPost("{id:guid}/return")]
    [ProducesResponseType(typeof(ResultViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReturnBook(Guid id)
    {
        var command = new ReturnBookCommand(id);
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
            return result.Message.Contains("not found") ? NotFound(result.Message) : BadRequest(result.Message);
            
        return Ok(result);
    }
}