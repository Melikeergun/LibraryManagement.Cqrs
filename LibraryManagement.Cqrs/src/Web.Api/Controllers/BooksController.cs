// src/Web.Api/Controllers/BooksController.cs
//
// ═══════════════════════════════════════════════════════════════
// WEB.API KATMANI — Controller
// ═══════════════════════════════════════════════════════════════
// Controller'ın tek görevi:
//   1. HTTP isteğini al
//   2. İstek verisinden Command veya Query oluştur
//   3. Dispatcher'a gönder
//   4. Sonucu HTTP yanıtına dönüştür
//
// İş mantığı YOKTUR. Domain kuralı YOKTUR.
// Handler kim, ne yapıyor — Controller bilmez.
// ═══════════════════════════════════════════════════════════════

using Application.Books.Commands.AddBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Queries.GetAllBooks;
using Application.Books.Queries.GetBookById;
using Application.Common.DTOs;
using Application.Common.Exceptions;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public BooksController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    // GET api/books
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        // Query oluştur → Dispatcher'a gönder
        var result = await _dispatcher.SendQueryAsync<List<BookResponseDto>>(
            new GetAllBooksQuery(), ct);
        return Ok(result);
    }

    // GET api/books/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        try
        {
            var result = await _dispatcher.SendQueryAsync<BookResponseDto>(
                new GetBookByIdQuery { Id = id }, ct);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // POST api/books
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] BookRequestDto dto, CancellationToken ct)
    {
        try
        {
            var command = new AddBookCommand
            {
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                TotalCopies = dto.TotalCopies
            };

            var result = await _dispatcher.SendCommandAsync<BookResponseDto>(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT api/books/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BookRequestDto dto, CancellationToken ct)
    {
        try
        {
            var command = new UpdateBookCommand
            {
                Id = id,
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                TotalCopies = dto.TotalCopies
            };

            var result = await _dispatcher.SendCommandAsync<BookResponseDto>(command, ct);
            return Ok(result);
        }
        catch (NotFoundException ex) { return NotFound(ex.Message); }
        catch (DomainException ex) { return BadRequest(ex.Message); }
    }

    // DELETE api/books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        try
        {
            await _dispatcher.SendCommandAsync<bool>(
                new DeleteBookCommand { Id = id }, ct);
            return NoContent();
        }
        catch (NotFoundException ex) { return NotFound(ex.Message); }
        catch (DomainException ex) { return BadRequest(ex.Message); }
    }
}
