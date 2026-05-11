// LibraryManagement.Crud/Services/LoanService.cs

// ⚠️ CRUD Mimarisinin Temel Özelliği:
// LoanService hem kitap ÖDÜNÇ VERME (yazma) hem de geciken kitapları LİSTELEME (okuma) işlemlerini yapıyor.
// İş kuralları arttıkça (gecikme cezası, üye limiti, bildirim vs.) bu sınıf hızla büyür.

using LibraryManagement.Crud.Data;
using LibraryManagement.Crud.DTOs;
using LibraryManagement.Crud.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Crud.Services;

public class LoanService
{
    private readonly AppDbContext _context;

    public LoanService(AppDbContext context)
    {
        _context = context;
    }

    // ── YAZMA İŞLEMLERİ ──────────────────────────────────────────────────────

    // Kitap ödünç verir.
    public async Task<LoanResponseDto> LoanBookAsync(LoanRequestDto dto)
    {
        var book = await _context.Books.FindAsync(dto.BookId);
        if (book == null)
            throw new InvalidOperationException("Kitap bulunamadı.");

        var member = await _context.Members.FindAsync(dto.MemberId);
        if (member == null)
            throw new InvalidOperationException("Üye bulunamadı.");

        // Domain kuralı: Müsait kopya yoksa ödünç verilemez.
        if (book.AvailableCopies <= 0)
            throw new InvalidOperationException("Bu kitabın müsait kopyası yok.");

        // Ödünç kaydı oluştur
        var loan = new Loan
        {
            BookId = dto.BookId,
            MemberId = dto.MemberId,
            LoanDate = DateTime.UtcNow,
            DueDate = dto.DueDate,
            ReturnDate = null    // Henüz iade edilmedi
        };

        // Domain kuralı: Ödünç verilince müsait kopya 1 azalır.
        book.AvailableCopies--;

        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();

        // Navigation property'leri manuel olarak ata
        loan.Book = book;
        loan.Member = member;

        return MapToResponseDto(loan);
    }

    // Kitabı iade eder.
    public async Task<LoanResponseDto> ReturnBookAsync(int loanId)
    {
        // Loan kaydını Book ve Member bilgileriyle birlikte getir
        var loan = await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .FirstOrDefaultAsync(l => l.Id == loanId);

        if (loan == null)
            throw new InvalidOperationException("Ödünç kaydı bulunamadı.");

        if (loan.ReturnDate != null)
            throw new InvalidOperationException("Bu kitap zaten iade edilmiş.");

        // Domain kuralı: İade edilince ReturnDate doldurulur.
        loan.ReturnDate = DateTime.UtcNow;

        // Domain kuralı: İade edilince müsait kopya 1 artar.
        loan.Book.AvailableCopies++;

        await _context.SaveChangesAsync();

        return MapToResponseDto(loan);
    }

    // ── OKUMA İŞLEMLERİ ──────────────────────────────────────────────────────

    // Aktif (iade edilmemiş) tüm ödünç kayıtlarını listeler.
    public async Task<List<LoanResponseDto>> GetActiveLoansAsync()
    {
        // ReturnDate null olan kayıtlar aktif ödünçtür.
        var loans = await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.ReturnDate == null)
            .ToListAsync();

        return loans.Select(MapToResponseDto).ToList();
    }

    // Geciken kitapları listeler.
    public async Task<List<LoanResponseDto>> GetOverdueLoansAsync()
    {
        var today = DateTime.UtcNow;

        // Domain kuralı: DueDate bugünden küçük ve ReturnDate null ise gecikmiş.
        var loans = await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.ReturnDate == null && l.DueDate < today)
            .ToListAsync();

        return loans.Select(MapToResponseDto).ToList();
    }

    // ── YARDIMCI METOT ───────────────────────────────────────────────────────

    private static LoanResponseDto MapToResponseDto(Loan loan)
    {
        return new LoanResponseDto
        {
            Id = loan.Id,
            BookTitle = loan.Book.Title,
            MemberFullName = loan.Member.FullName,
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnDate = loan.ReturnDate
        };
    }
}
