// src/Application/Loans/Queries/GetActiveLoans/GetActiveLoansQuery.cs
// src/Application/Loans/Queries/GetOverdueLoans/GetOverdueLoansQuery.cs
//
// ═══════════════════════════════════════════════════════════════
// OKUMA TARAFI — Ödünç Sorguları
// ═══════════════════════════════════════════════════════════════
// Bu iki Query Handler sadece okur.
// LoanBookCommandHandler ile hiçbir kod paylaşmazlar.
// Biri değişirse diğeri etkilenmez.
// ═══════════════════════════════════════════════════════════════

using Application.Common.DTOs;
using Domain.Interfaces;
using SharedKernel;

// ── AKTİF ÖDÜNÇLER ───────────────────────────────────────────────────────────

namespace Application.Loans.Queries.GetActiveLoans;

public class GetActiveLoansQuery : IQuery<List<LoanResponseDto>> { }

public class GetActiveLoansQueryHandler : IQueryHandler<GetActiveLoansQuery, List<LoanResponseDto>>
{
    private readonly ILoanRepository _loanRepository;

    public GetActiveLoansQueryHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<List<LoanResponseDto>> HandleAsync(GetActiveLoansQuery query, CancellationToken ct = default)
    {
        var loans = await _loanRepository.GetActiveLoansAsync(ct);

        return loans.Select(l => new LoanResponseDto
        {
            Id = l.Id,
            BookTitle = l.Book.Title,
            MemberFullName = l.Member.FullName,
            LoanDate = l.LoanDate,
            DueDate = l.DueDate,
            ReturnDate = l.ReturnDate
        }).ToList();
    }
}

// ── GECİKEN ÖDÜNÇLER ─────────────────────────────────────────────────────────

namespace Application.Loans.Queries.GetOverdueLoans;

public class GetOverdueLoansQuery : IQuery<List<LoanResponseDto>> { }

public class GetOverdueLoansQueryHandler : IQueryHandler<GetOverdueLoansQuery, List<LoanResponseDto>>
{
    private readonly ILoanRepository _loanRepository;

    public GetOverdueLoansQueryHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<List<LoanResponseDto>> HandleAsync(GetOverdueLoansQuery query, CancellationToken ct = default)
    {
        // Repository sadece geciken kayıtları filtreler
        var loans = await _loanRepository.GetOverdueLoansAsync(ct);

        return loans.Select(l => new LoanResponseDto
        {
            Id = l.Id,
            BookTitle = l.Book.Title,
            MemberFullName = l.Member.FullName,
            LoanDate = l.LoanDate,
            DueDate = l.DueDate,
            ReturnDate = l.ReturnDate
        }).ToList();
    }
}
