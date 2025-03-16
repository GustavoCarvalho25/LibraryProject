using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LibraryProject.Controllers;

[ApiController]
[Route("/api/books")]
public class BookController : ControllerBase
{
    public BookController()
    {
        
    }

    public async Task<IActionResult> Post(Book book)
    {
        return Ok(book);
    }
    
}