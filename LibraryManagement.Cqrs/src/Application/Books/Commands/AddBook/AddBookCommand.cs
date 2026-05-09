// src/Application/Books/Commands/AddBook/AddBookCommand.cs
// src/Application/Books/Commands/AddBook/AddBookCommandHandler.cs
//
// ═══════════════════════════════════════════════════════════════
// CQRS — COMMAND TARAFI (YAZMA)
// ═══════════════════════════════════════════════════════════════
// Bu dosyada iki şey var:
//   1. AddBookCommand    → "Kitap eklemek istiyorum" talebi
//   2. AddBookCommandHandler → Bu talebi işleyen TEK sınıf
//
// CRUD farkı:
//   CRUD'da BookService.AddBookAsync() hem ekleme hem listeleme yapıyordu.
//   Burada AddBookCommandHandler SADECE ekleme işini bilir.
//   Listeleme Handler'ından tamamen habersizdir.
// ═══════════════════════════════════════════════════════════════

using Application.Common.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using SharedKernel;

namespace Application.Books.Commands.AddBook;

// ── COMMAND ──────────────────────────────────────────────────────────────────
// Veri taşıyıcı: Controller bu nesneyi oluşturup Dispatcher'a gönderir.

public class AddBookCommand : ICommand<BookResponseDto>
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
}

// ── COMMAND HANDLER ───────────────────────────────────────────────────────────
// Bu sınıfın tek sorumluluğu: kitap eklemek.

public class AddBookCommandHandler : ICommandHandler<AddBookCommand, BookResponseDto>
{
    private readonly IBookRepository _bookRepository;

    // IBookRepository inject ediliyor — EF Core değil, soyutlama.
    // Bu sayede Handler, Infrastructure katmanını tanımaz.
    public AddBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookResponseDto> HandleAsync(AddBookCommand command, CancellationToken ct = default)
    {
        // Domain entity'yi factory method ile oluştur
        // İş kuralı doğrulamaları Book.Create içinde yapılır
        var book = Book.Create(command.Title, command.Author, command.ISBN, command.TotalCopies);

        await _bookRepository.AddAsync(book, ct);

        // Entity → DTO dönüşümü
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
