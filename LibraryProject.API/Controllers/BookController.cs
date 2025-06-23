using Application.Commands.Books.AddBookCommand;
using Application.Commands.Books.LoanBookCommand;
using Application.Commands.Books.RemoveBookCommand;
using Application.Commands.Books.ReturnBookCommand;
using Application.Commands.Books.UpdateBookCommand;
using Application.Models;
using Application.Queries.Books;
using Application.ViewModels;
using Core.Shared;
using LibraryProject.ModelBinders;
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
    [ProducesResponseType(typeof(ResultViewModel<PagedResult<BookViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([ModelBinder(typeof(QueryOptionsModelBinder))] QueryOptions options)
    {
        var query = new GetAllBooksQuery(options);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(result.Message);
            
        return Ok(result);
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
    [ProducesResponseType(typeof(ResultViewModel<PagedResult<BookViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByAuthor([ModelBinder(typeof(QueryOptionsModelBinder))] QueryOptions options, string authorName)
    {
        var query = new GetBooksByAuthorQuery(authorName, options);
        var result = await _mediator.Send(query);
        
        if (result.IsSuccess)
            return BadRequest(result.Message);
            
        return Ok(result);
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
        command.Id = id;
            
        var result = await _mediator.Send(command);
        
        if (!result.IsSuccess)
            return BadRequest(result.Message);
            
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
            return BadRequest(result.Message);
            
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
            return NotFound(result.Message);
            
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
            return BadRequest(result.Message);
            
        return Ok(result);
    }
}