// src/Application/Books/Commands/UpdateBook/UpdateBookCommand.cs

using Application.Common.DTOs;
using Application.Common.Exceptions;
using Domain.Interfaces;
using SharedKernel;

namespace Application.Books.Commands.UpdateBook;

// ── COMMAND ──────────────────────────────────────────────────────────────────

public class UpdateBookCommand : ICommand<BookResponseDto>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
}

// ── COMMAND HANDLER ───────────────────────────────────────────────────────────
// Tek sorumluluğu: kitap güncellemek.

public class UpdateBookCommandHandler : ICommandHandler<UpdateBookCommand, BookResponseDto>
{
    private readonly IBookRepository _bookRepository;

    public UpdateBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookResponseDto> HandleAsync(UpdateBookCommand command, CancellationToken ct = default)
    {
        var book = await _bookRepository.GetByIdAsync(command.Id, ct);

        // Kitap yoksa Application katmanı exception fırlatır
        if (book == null)
            throw new NotFoundException(nameof(book), command.Id);

        // Güncelleme iş kuralı entity'nin kendi metodunda
        book.Update(command.Title, command.Author, command.ISBN, command.TotalCopies);

        await _bookRepository.UpdateAsync(book, ct);

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
