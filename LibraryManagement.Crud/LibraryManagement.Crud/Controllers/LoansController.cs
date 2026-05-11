// LibraryManagement.Crud/Controllers/LoansController.cs

using LibraryManagement.Crud.DTOs;
using LibraryManagement.Crud.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Crud.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly LoanService _loanService;

    public LoansController(LoanService loanService)
    {
        _loanService = loanService;
    }

    // POST api/loans
    [HttpPost]
    public async Task<IActionResult> LoanBook([FromBody] LoanRequestDto dto)
    {
        try
        {
            var loan = await _loanService.LoanBookAsync(dto);
            return Ok(loan);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT api/loans/5/return
    [HttpPut("{id}/return")]
    public async Task<IActionResult> ReturnBook(int id)
    {
        try
        {
            var loan = await _loanService.ReturnBookAsync(id);
            return Ok(loan);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET api/loans/active
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveLoans()
    {
        var loans = await _loanService.GetActiveLoansAsync();
        return Ok(loans);
    }

    // GET api/loans/overdue
    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdueLoans()
    {
        var loans = await _loanService.GetOverdueLoansAsync();
        return Ok(loans);
    }
}
