// src/Domain/Interfaces/IBookRepository.cs
//
// ═══════════════════════════════════════════════════════════════
// DOMAIN — Repository Arayüzleri
// ═══════════════════════════════════════════════════════════════
// Repository pattern: veritabanı erişimini soyutlar.
//
// CRUD projesinde yoktu. Handler'lar DbContext'e direkt erişiyordu.
// Burada Handler'lar bu arayüzleri kullanır.
// Gerçek implementasyon Infrastructure katmanındadır.
//
// Bu sayede:
//   - Domain ve Application katmanları EF Core'u tanımaz
//   - Veritabanı değişse (SQLite → PostgreSQL) sadece Infrastructure değişir
//   - Test yazarken gerçek DB yerine sahte (mock) repository kullanılabilir
// ═══════════════════════════════════════════════════════════════

using Domain.Entities;

namespace Domain.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Book>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Book book, CancellationToken ct = default);
    Task UpdateAsync(Book book, CancellationToken ct = default);
    Task DeleteAsync(Book book, CancellationToken ct = default);
    Task<bool> HasActiveLoanAsync(int bookId, CancellationToken ct = default);
}

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Member>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Member member, CancellationToken ct = default);
}

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<Loan>> GetActiveLoansAsync(CancellationToken ct = default);
    Task<List<Loan>> GetOverdueLoansAsync(CancellationToken ct = default);
    Task AddAsync(Loan loan, CancellationToken ct = default);
    Task UpdateAsync(Loan loan, CancellationToken ct = default);
}
