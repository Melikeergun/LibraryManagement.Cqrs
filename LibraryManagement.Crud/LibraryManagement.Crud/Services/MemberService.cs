// LibraryManagement.Crud/Services/MemberService.cs

using LibraryManagement.Crud.Data;
using LibraryManagement.Crud.DTOs;
using LibraryManagement.Crud.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Crud.Services;

public class MemberService
{
    private readonly AppDbContext _context;

    public MemberService(AppDbContext context)
    {
        _context = context;
    }

    // ── YAZMA İŞLEMLERİ ──────────────────────────────────────────────────────

    // Yeni üye ekler.
    public async Task<MemberResponseDto> AddMemberAsync(MemberRequestDto dto)
    {
        var member = new Member
        {
            FullName = dto.FullName,
            Email = dto.Email
        };

        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        return MapToResponseDto(member);
    }

    // ── OKUMA İŞLEMLERİ ──────────────────────────────────────────────────────

    // Tüm üyeleri listeler.
    public async Task<List<MemberResponseDto>> GetAllMembersAsync()
    {
        var members = await _context.Members.ToListAsync();
        return members.Select(MapToResponseDto).ToList();
    }

    // ── YARDIMCI METOT ───────────────────────────────────────────────────────

    private static MemberResponseDto MapToResponseDto(Member member)
    {
        return new MemberResponseDto
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email
        };
    }
}
