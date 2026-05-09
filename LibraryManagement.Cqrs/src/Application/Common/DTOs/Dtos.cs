// src/Application/Common/DTOs/BookDtos.cs
// src/Application/Common/DTOs/MemberDtos.cs
// src/Application/Common/DTOs/LoanDtos.cs
//
// DTO (Data Transfer Object): katmanlar arası veri taşıyan sade nesneler.
// Domain entity'leri dışarıya doğrudan verilmez — DTO'ya dönüştürülür.
// Bu sayede API çıktısı domain modelinden bağımsız şekilde şekillendirilebilir.

namespace Application.Common.DTOs;

// ── BOOK ─────────────────────────────────────────────────────────────────────

/// <summary>Kitap ekleme ve güncelleme için gelen istek verisi</summary>
public class BookRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
}

/// <summary>Kullanıcıya döndürülen kitap verisi</summary>
public class BookResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}

// ── MEMBER ───────────────────────────────────────────────────────────────────

/// <summary>Üye ekleme için gelen istek verisi</summary>
public class MemberRequestDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

/// <summary>Kullanıcıya döndürülen üye verisi</summary>
public class MemberResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

// ── LOAN ─────────────────────────────────────────────────────────────────────

/// <summary>Ödünç verme için gelen istek verisi</summary>
public class LoanRequestDto
{
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime DueDate { get; set; }
}

/// <summary>Kullanıcıya döndürülen ödünç kaydı</summary>
public class LoanResponseDto
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string MemberFullName { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    // Hesaplanan alan: ReturnDate yoksa aktif
    public bool IsActive => ReturnDate == null;

    // Hesaplanan alan: gecikmiş mi?
    public bool IsOverdue => ReturnDate == null && DueDate < DateTime.UtcNow;
}
