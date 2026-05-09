// src/Application/Books/Commands/DeleteBook/DeleteBookCommand.cs

using Application.Common.Exceptions;
using Domain.Exceptions;
using Domain.Interfaces;
using SharedKernel;

namespace Application.Books.Commands.DeleteBook;

// ── COMMAND ──────────────────────────────────────────────────────────────────

public class DeleteBookCommand : ICommand<bool>
{
    public int Id { get; set; }
}

// ── COMMAND HANDLER ───────────────────────────────────────────────────────────
// Tek sorumluluğu: kitap silmek.
// Domain kuralı: aktif ödünçte olan kitap silinemez.

public class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand, bool>
{
    private readonly IBookRepository _bookRepository;

    public DeleteBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<bool> HandleAsync(DeleteBookCommand command, CancellationToken ct = default)
    {
        var book = await _bookRepository.GetByIdAsync(command.Id, ct);

        if (book == null)
            throw new NotFoundException(nameof(book), command.Id);

        // Domain kuralı kontrolü: aktif ödünç var mı?
        bool hasActiveLoan = await _bookRepository.HasActiveLoanAsync(command.Id, ct);

        if (hasActiveLoan)
            throw new DomainException("Aktif ödünçte olan kitap silinemez.");

        await _bookRepository.DeleteAsync(book, ct);

        return true;
    }
}
