// LibraryManagement.Crud/Services/BookService.cs

// ⚠️ CRUD Mimarisinin Temel Özelliği:
// BookService hem kitap EKLEME (yazma) hem de kitap LİSTELEME (okuma) işlemlerini yapıyor.
// Okuma ve yazma sorumlulukları aynı sınıf içinde bir arada bulunuyor.
// Proje büyüdükçe bu sınıfa daha fazla metot eklenir ve sınıf şişer.

using LibraryManagement.Crud.Data;
using LibraryManagement.Crud.DTOs;
using LibraryManagement.Crud.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Crud.Services;

public class BookService
{
    private readonly AppDbContext _context;

    public BookService(AppDbContext context)
    {
        _context = context;
    }

    // ── write operation ──────────────────────────────────────────────────────

    // Yeni kitap ekler.
    public async Task<BookResponseDto> AddBookAsync(BookRequestDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            ISBN = dto.ISBN,
            TotalCopies = dto.TotalCopies,
            AvailableCopies = dto.TotalCopies   // Başlangıçta tüm kopyalar müsait
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return MapToResponseDto(book);
    }

    // Mevcut kitabı günceller.
    public async Task<BookResponseDto?> UpdateBookAsync(int id, BookRequestDto dto)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return null;

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.ISBN = dto.ISBN;
        book.TotalCopies = dto.TotalCopies;

        await _context.SaveChangesAsync();

        return MapToResponseDto(book);
    }

    // Kitabı siler. Aktif ödünçte olan kitap silinemez.
    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        // Domain kuralı: Aktif ödünçte olan kitap silinemez.
        bool hasActiveLoan = await _context.Loans
            .AnyAsync(l => l.BookId == id && l.ReturnDate == null);

        if (hasActiveLoan)
            throw new InvalidOperationException("Aktif ödünçte olan kitap silinemez.");

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return true;
    }

    // ── read operations ──────────────────────────────────────────────────────

    // Tüm kitapları listeler.
    public async Task<List<BookResponseDto>> GetAllBooksAsync()
    {
        var books = await _context.Books.ToListAsync();
        return books.Select(MapToResponseDto).ToList();
    }

    // Belirli bir kitabın detayını getirir.
    public async Task<BookResponseDto?> GetBookByIdAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return null;

        return MapToResponseDto(book);
    }

    // ── YARDIMCI METOT ───────────────────────────────────────────────────────

    // Book modelini BookResponseDto'ya dönüştürür.
    private static BookResponseDto MapToResponseDto(Book book)
    {
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
