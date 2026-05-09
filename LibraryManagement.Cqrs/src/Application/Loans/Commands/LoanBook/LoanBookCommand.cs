// src/Application/Loans/Commands/LoanBook/LoanBookCommand.cs
//
// ═══════════════════════════════════════════════════════════════
// EN KARMAŞIK COMMAND: Kitap Ödünç Verme
// ═══════════════════════════════════════════════════════════════
// Bu Handler birden fazla repository kullanır:
//   - IBookRepository  → Kitabı bul, kopyayı azalt
//   - IMemberRepository → Üyeyi bul
//   - ILoanRepository  → Ödünç kaydını oluştur
//
// CRUD farkı:
//   CRUD'da LoanService içinde hem bu işlem hem listeleme vardı.
//   Burada LoanBookCommandHandler sadece "ödünç ver" işini bilir.
//   Ödünç listeleme QueryHandler'larından tamamen bağımsızdır.
//
// Domain katmanı ne yapıyor?
//   - Book.DecreaseCopy()  → iş kuralını entity uygular
//   - Loan.Create()        → geçerlilik kontrolü entity'de
//   Handler sadece orkestrasyon yapar.
// ═══════════════════════════════════════════════════════════════

using Application.Common.DTOs;
using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using SharedKernel;

namespace Application.Loans.Commands.LoanBook;

// ── COMMAND ──────────────────────────────────────────────────────────────────

public class LoanBookCommand : ICommand<LoanResponseDto>
{
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime DueDate { get; set; }
}

// ── COMMAND HANDLER ───────────────────────────────────────────────────────────

public class LoanBookCommandHandler : ICommandHandler<LoanBookCommand, LoanResponseDto>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ILoanRepository _loanRepository;

    public LoanBookCommandHandler(
        IBookRepository bookRepository,
        IMemberRepository memberRepository,
        ILoanRepository loanRepository)
    {
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
        _loanRepository = loanRepository;
    }

    public async Task<LoanResponseDto> HandleAsync(LoanBookCommand command, CancellationToken ct = default)
    {
        // 1. Kitabı bul
        var book = await _bookRepository.GetByIdAsync(command.BookId, ct);
        if (book == null)
            throw new NotFoundException("Kitap", command.BookId);

        // 2. Üyeyi bul
        var member = await _memberRepository.GetByIdAsync(command.MemberId, ct);
        if (member == null)
            throw new NotFoundException("Üye", command.MemberId);

        // 3. Domain kuralı: müsait kopya var mı?
        //    Bu kontrol Book.DecreaseCopy() içinde yapılır.
        //    Handler iş kuralını bilmez; entity bilir.
        book.DecreaseCopy();   // DomainException fırlatabilir

        // 4. Ödünç kaydını oluştur
        var loan = Loan.Create(command.BookId, command.MemberId, command.DueDate);

        // 5. Veritabanına kaydet
        await _bookRepository.UpdateAsync(book, ct);
        await _loanRepository.AddAsync(loan, ct);

        return new LoanResponseDto
        {
            Id = loan.Id,
            BookTitle = book.Title,
            MemberFullName = member.FullName,
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnDate = loan.ReturnDate
        };
    }
}
