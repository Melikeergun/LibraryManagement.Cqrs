// LibraryManagement.Crud/DTOs/LoanDtos.cs

namespace LibraryManagement.Crud.DTOs;

// Kitap ödünç verme için kullanıcıdan alınan veriler
public class LoanRequestDto
{
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime DueDate { get; set; }   // Son iade tarihi
}

// Kullanıcıya döndürülen ödünç kaydı
public class LoanResponseDto
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string MemberFullName { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    // ReturnDate yoksa kitap hâlâ ödünçte
    public bool IsActive => ReturnDate == null;
}
