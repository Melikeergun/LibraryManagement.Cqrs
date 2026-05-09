// src/Application/Loans/Commands/ReturnBook/ReturnBookCommand.cs

using Application.Common.DTOs;
using Application.Common.Exceptions;
using Domain.Interfaces;
using SharedKernel;

namespace Application.Loans.Commands.ReturnBook;

// ── COMMAND ──────────────────────────────────────────────────────────────────

public class ReturnBookCommand : ICommand<LoanResponseDto>
{
    public int LoanId { get; set; }
}

// ── COMMAND HANDLER ───────────────────────────────────────────────────────────
// Tek sorumluluğu: kitabı iade etmek.

public class ReturnBookCommandHandler : ICommandHandler<ReturnBookCommand, LoanResponseDto>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;

    public ReturnBookCommandHandler(ILoanRepository loanRepository, IBookRepository bookRepository)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
    }

    public async Task<LoanResponseDto> HandleAsync(ReturnBookCommand command, CancellationToken ct = default)
    {
        // Ödünç kaydını bul
        var loan = await _loanRepository.GetByIdAsync(command.LoanId, ct);
        if (loan == null)
            throw new NotFoundException("Ödünç kaydı", command.LoanId);

        // Domain kuralı: iade et (zaten iade edilmişse DomainException fırlatır)
        loan.Return();

        // Kitabın müsait kopyasını artır
        var book = await _bookRepository.GetByIdAsync(loan.BookId, ct);
        if (book != null)
        {
            book.IncreaseCopy();
            await _bookRepository.UpdateAsync(book, ct);
        }

        await _loanRepository.UpdateAsync(loan, ct);

        return new LoanResponseDto
        {
            Id = loan.Id,
            BookTitle = loan.Book?.Title ?? string.Empty,
            MemberFullName = loan.Member?.FullName ?? string.Empty,
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnDate = loan.ReturnDate
        };
    }
}
