// LibraryManagement.Crud/Controllers/BooksController.cs

using LibraryManagement.Crud.DTOs;
using LibraryManagement.Crud.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Crud.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    // GET api/books
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    // GET api/books/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    // POST api/books
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] BookRequestDto dto)
    {
        var book = await _bookService.AddBookAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT api/books/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BookRequestDto dto)
    {
        var book = await _bookService.UpdateBookAsync(id, dto);
        if (book == null) return NotFound();
        return Ok(book);
    }

    // DELETE api/books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            // Domain kuralı ihlali: 400 Bad Request döndür
            return BadRequest(ex.Message);
        }
    }
}
