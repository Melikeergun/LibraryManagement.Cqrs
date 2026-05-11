// LibraryManagement.Crud/Models/Book.cs

namespace LibraryManagement.Crud.Models;

// Kütüphanedeki kitabı temsil eder.
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;

    // Kitabın toplam kopya sayısı
    public int TotalCopies { get; set; }

    // Şu an ödünç verilebilir kopya sayısı
    public int AvailableCopies { get; set; }
}
