// src/Web.Api/Controllers/LoansController.cs

using Application.Common.DTOs;
using Application.Common.Exceptions;
using Application.Loans.Commands.LoanBook;
using Application.Loans.Commands.ReturnBook;
using Application.Loans.Queries.GetActiveLoans;
using Application.Loans.Queries.GetOverdueLoans;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public LoansController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    // POST api/loans  — Kitap ödünç ver
    [HttpPost]
    public async Task<IActionResult> LoanBook([FromBody] LoanRequestDto dto, CancellationToken ct)
    {
        try
        {
            var command = new LoanBookCommand
            {
                BookId = dto.BookId,
                MemberId = dto.MemberId,
                DueDate = dto.DueDate
            };

            var result = await _dispatcher.SendCommandAsync<LoanResponseDto>(command, ct);
            return Ok(result);
        }
        catch (NotFoundException ex) { return NotFound(ex.Message); }
        catch (DomainException ex) { return BadRequest(ex.Message); }
    }

    // PUT api/loans/5/return  — Kitap iade et
    [HttpPut("{id}/return")]
    public async Task<IActionResult> ReturnBook(int id, CancellationToken ct)
    {
        try
        {
            var result = await _dispatcher.SendCommandAsync<LoanResponseDto>(
                new ReturnBookCommand { LoanId = id }, ct);
            return Ok(result);
        }
        catch (NotFoundException ex) { return NotFound(ex.Message); }
        catch (DomainException ex) { return BadRequest(ex.Message); }
    }

    // GET api/loans/active  — Aktif ödünçler
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveLoans(CancellationToken ct)
    {
        var result = await _dispatcher.SendQueryAsync<List<LoanResponseDto>>(
            new GetActiveLoansQuery(), ct);
        return Ok(result);
    }

    // GET api/loans/overdue  — Geciken kitaplar
    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdueLoans(CancellationToken ct)
    {
        var result = await _dispatcher.SendQueryAsync<List<LoanResponseDto>>(
            new GetOverdueLoansQuery(), ct);
        return Ok(result);
    }
}
