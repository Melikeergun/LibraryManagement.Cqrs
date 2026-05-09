// src/Application/Books/Queries/GetAllBooks/GetAllBooksQuery.cs
//
// ═══════════════════════════════════════════════════════════════
// CQRS — QUERY TARAFI (OKUMA)
// ═══════════════════════════════════════════════════════════════
// Query Handler'lar SADECE okur. Hiçbir zaman veri değiştirmez.
//
// CRUD farkı:
//   CRUD'da BookService.GetAllBooksAsync() ekleme ile aynı sınıftaydı.
//   Burada GetAllBooksQueryHandler bağımsız bir sınıf.
//   AddBookCommandHandler değişirse bu sınıf hiç etkilenmez.
// ═══════════════════════════════════════════════════════════════

using Application.Common.DTOs;
using Domain.Interfaces;
using SharedKernel;

namespace Application.Books.Queries.GetAllBooks;

// ── QUERY ─────────────────────────────────────────────────────────────────────
// Parametre gerekmez; tüm kitapları getir.

public class GetAllBooksQuery : IQuery<List<BookResponseDto>> { }

// ── QUERY HANDLER ─────────────────────────────────────────────────────────────

public class GetAllBooksQueryHandler : IQueryHandler<GetAllBooksQuery, List<BookResponseDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<BookResponseDto>> HandleAsync(GetAllBooksQuery query, CancellationToken ct = default)
    {
        var books = await _bookRepository.GetAllAsync(ct);

        // Entity listesini DTO listesine dönüştür
        return books.Select(book => new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies
        }).ToList();
    }
}
