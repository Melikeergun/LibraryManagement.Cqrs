// LibraryManagement.Crud/DTOs/BookDtos.cs

namespace LibraryManagement.Crud.DTOs;

// Kitap ekleme ve güncelleme için kullanıcıdan alınan veriler
public class BookRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
}

// Kullanıcıya döndürülen kitap verisi
public class BookResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}
