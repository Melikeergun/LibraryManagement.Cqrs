// src/Domain/Entities/Loan.cs
//
// Bir ödünç kaydını temsil eden entity.
// "Aktif mi?", "Gecikmiş mi?" soruları bu entity'nin kendi metodlarında yanıt bulur.
// Handler ya da servis bu hesaplamayı yapmaz — entity bilir.

using SharedKernel;
using Domain.Exceptions;

namespace Domain.Entities;

public class Loan : BaseEntity
{
    public int BookId { get; private set; }
    public Book Book { get; private set; } = null!;

    public int MemberId { get; private set; }
    public Member Member { get; private set; } = null!;

    public DateTime LoanDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }   // Null = hâlâ ödünçte

    protected Loan() { }

    public static Loan Create(int bookId, int memberId, DateTime dueDate)
    {
        if (dueDate <= DateTime.UtcNow)
            throw new DomainException("Son iade tarihi bugünden ileri bir tarih olmalıdır.");

        return new Loan
        {
            BookId = bookId,
            MemberId = memberId,
            LoanDate = DateTime.UtcNow,
            DueDate = dueDate,
            ReturnDate = null
        };
    }

    // ── Domain Sorguları ─────────────────────────────────────────
    // Bu metotlar veriye BAKAR ama değiştirmez → Query tarafı için ideal

    /// <summary>
    /// ReturnDate null ise ödünç kaydı aktiftir.
    /// </summary>
    public bool IsActive() => ReturnDate == null;

    /// <summary>
    /// DueDate geçmiş ve henüz iade edilmemişse gecikmiştir.
    /// </summary>
    public bool IsOverdue() => ReturnDate == null && DueDate < DateTime.UtcNow;

    // ── Domain Komutu ────────────────────────────────────────────

    /// <summary>
    /// Kitabı iade eder. ReturnDate doldurulur.
    /// Zaten iade edilmişse DomainException fırlatılır.
    /// </summary>
    public void Return()
    {
        if (ReturnDate != null)
            throw new DomainException("Bu ödünç kaydı zaten iade edilmiş.");

        ReturnDate = DateTime.UtcNow;
    }
}
