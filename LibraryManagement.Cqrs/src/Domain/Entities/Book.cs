// src/Domain/Entities/Book.cs
//
// ═══════════════════════════════════════════════════════════════
// DOMAIN KATMANI — Entity
// ═══════════════════════════════════════════════════════════════
// Domain katmanı projenin kalbidir. Hiçbir dış katmana bağımlı
// değildir: ne EF Core bilir, ne Controller, ne servis.
//
// Entity = kimliği (Id) olan, iş kurallarını kendi içinde
// barındıran nesne. Book bir entity'dir çünkü Id'si var ve
// "kaç kopya ödünç verilebilir?" gibi kuralları kendisi bilir.
//
// CRUD projesindeki fark:
//   CRUD'da Book sadece veri taşıyan düz bir class'tı.
//   Burada Book kendi iş kurallarını (AvailableCopies azalt/artır)
//   metot olarak barındırır. Buna "Rich Domain Model" denir.
// ═══════════════════════════════════════════════════════════════

using SharedKernel;
using Domain.Exceptions;

namespace Domain.Entities;

public class Book : BaseEntity
{
    // ── Alanlar ──────────────────────────────────────────────────

    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public string ISBN { get; private set; } = string.Empty;
    public int TotalCopies { get; private set; }
    public int AvailableCopies { get; private set; }

    // EF Core için parametresiz constructor gerekir (protected yeterli)
    protected Book() { }

    // ── Factory Method ───────────────────────────────────────────
    // new Book() yerine Book.Create() kullanılır.
    // Nesne her zaman geçerli bir durumda oluşturulur.

    public static Book Create(string title, string author, string isbn, int totalCopies)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Kitap başlığı boş olamaz.");

        if (totalCopies <= 0)
            throw new DomainException("Kopya sayısı 0'dan büyük olmalıdır.");

        return new Book
        {
            Title = title,
            Author = author,
            ISBN = isbn,
            TotalCopies = totalCopies,
            AvailableCopies = totalCopies   // Başlangıçta tümü müsait
        };
    }

    // ── Domain Metotları ─────────────────────────────────────────
    // İş kuralları burada yaşar. Servis ya da Handler'a sızmaz.

    /// <summary>
    /// Kitap ödünç verildiğinde çağrılır. Müsait kopya 1 azalır.
    /// </summary>
    public void DecreaseCopy()
    {
        // Domain kuralı: Müsait kopya 0 ise ödünç verilemez.
        if (AvailableCopies <= 0)
            throw new DomainException($"'{Title}' kitabının müsait kopyası kalmadı.");

        AvailableCopies--;
    }

    /// <summary>
    /// Kitap iade edildiğinde çağrılır. Müsait kopya 1 artar.
    /// </summary>
    public void IncreaseCopy()
    {
        if (AvailableCopies >= TotalCopies)
            throw new DomainException("Müsait kopya sayısı toplam kopya sayısını aşamaz.");

        AvailableCopies++;
    }

    /// <summary>
    /// Kitap bilgileri güncellenir.
    /// </summary>
    public void Update(string title, string author, string isbn, int totalCopies)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Kitap başlığı boş olamaz.");

        Title = title;
        Author = author;
        ISBN = isbn;
        TotalCopies = totalCopies;
    }
}
