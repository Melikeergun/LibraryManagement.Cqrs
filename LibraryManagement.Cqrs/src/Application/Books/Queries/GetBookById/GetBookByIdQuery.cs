// src/Application/Books/Queries/GetBookById/GetBookByIdQuery.cs

using Application.Common.DTOs;
using Application.Common.Exceptions;
using Domain.Interfaces;
using SharedKernel;

namespace Application.Books.Queries.GetBookById;

// ── QUERY ─────────────────────────────────────────────────────────────────────

public class GetBookByIdQuery : IQuery<BookResponseDto>
{
    public int Id { get; set; }
}

// ── QUERY HANDLER ─────────────────────────────────────────────────────────────

public class GetBookByIdQueryHandler : IQueryHandler<GetBookByIdQuery, BookResponseDto>
{
    private readonly IBookRepository _bookRepository;

    public GetBookByIdQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookResponseDto> HandleAsync(GetBookByIdQuery query, CancellationToken ct = default)
    {
        var book = await _bookRepository.GetByIdAsync(query.Id, ct);

        if (book == null)
            throw new NotFoundException(nameof(book), query.Id);

        return new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies
        };
    }
}
