// LibraryManagement.Crud/DTOs/MemberDtos.cs

namespace LibraryManagement.Crud.DTOs;

// Üye ekleme için kullanıcıdan alınan veriler
public class MemberRequestDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

// Kullanıcıya döndürülen üye verisi
public class MemberResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
