// src/Infrastructure/Persistence/Repositories/BookRepository.cs
// src/Infrastructure/Persistence/Repositories/MemberRepository.cs
// src/Infrastructure/Persistence/Repositories/LoanRepository.cs
//
// ═══════════════════════════════════════════════════════════════
// REPOSITORY IMPLEMENTASYONLARI
// ═══════════════════════════════════════════════════════════════
// Domain'de tanımlanan arayüzlerin gerçek EF Core implementasyonu.
//
// Application katmanı IBookRepository'yi bilir.
// Bu sınıfın var olduğunu bile bilmez.
// Program.cs'te "IBookRepository istenirse BookRepository ver" denir.
// ═══════════════════════════════════════════════════════════════

using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

// ── BOOK REPOSITORY ───────────────────────────────────────────────────────────

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Books.FindAsync(new object[] { id }, ct);

    public async Task<List<Book>> GetAllAsync(CancellationToken ct = default)
        => await _context.Books.ToListAsync(ct);

    public async Task AddAsync(Book book, CancellationToken ct = default)
    {
        await _context.Books.AddAsync(book, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Book book, CancellationToken ct = default)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Book book, CancellationToken ct = default)
    {
        _context.Books.Remove(book);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> HasActiveLoanAsync(int bookId, CancellationToken ct = default)
        => await _context.Loans.AnyAsync(l => l.BookId == bookId && l.ReturnDate == null, ct);
}

// ── MEMBER REPOSITORY ─────────────────────────────────────────────────────────

public class MemberRepository : IMemberRepository
{
    private readonly AppDbContext _context;

    public MemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Member?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Members.FindAsync(new object[] { id }, ct);

    public async Task<List<Member>> GetAllAsync(CancellationToken ct = default)
        => await _context.Members.ToListAsync(ct);

    public async Task AddAsync(Member member, CancellationToken ct = default)
    {
        await _context.Members.AddAsync(member, ct);
        await _context.SaveChangesAsync(ct);
    }
}

// ── LOAN REPOSITORY ───────────────────────────────────────────────────────────

public class LoanRepository : ILoanRepository
{
    private readonly AppDbContext _context;

    public LoanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Loan?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .FirstOrDefaultAsync(l => l.Id == id, ct);

    public async Task<List<Loan>> GetActiveLoansAsync(CancellationToken ct = default)
        => await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.ReturnDate == null)
            .ToListAsync(ct);

    public async Task<List<Loan>> GetOverdueLoansAsync(CancellationToken ct = default)
    {
        var today = DateTime.UtcNow;
        return await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.ReturnDate == null && l.DueDate < today)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Loan loan, CancellationToken ct = default)
    {
        await _context.Loans.AddAsync(loan, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Loan loan, CancellationToken ct = default)
    {
        _context.Loans.Update(loan);
        await _context.SaveChangesAsync(ct);
    }
}
