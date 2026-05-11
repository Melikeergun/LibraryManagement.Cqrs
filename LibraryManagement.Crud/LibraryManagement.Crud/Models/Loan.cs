// LibraryManagement.Crud/Models/Loan.cs

namespace LibraryManagement.Crud.Models;

// Bir kitabın bir üyeye ödünç verilmesini temsil eder.
public class Loan
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;     // Navigation property

    public int MemberId { get; set; }
    public Member Member { get; set; } = null!; // Navigation property

    public DateTime LoanDate { get; set; }      // Ödünç verilen tarih
    public DateTime DueDate { get; set; }       // Son iade tarihi

    // Null ise kitap hâlâ ödünçte demektir.
    public DateTime? ReturnDate { get; set; }
}
