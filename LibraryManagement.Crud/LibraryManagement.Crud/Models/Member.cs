// LibraryManagement.Crud/Models/Member.cs

namespace LibraryManagement.Crud.Models;

// Kütüphane üyesini temsil eder.
public class Member
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
