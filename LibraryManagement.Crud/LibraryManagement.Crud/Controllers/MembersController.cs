// LibraryManagement.Crud/Controllers/MembersController.cs

using LibraryManagement.Crud.DTOs;
using LibraryManagement.Crud.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Crud.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly MemberService _memberService;

    public MembersController(MemberService memberService)
    {
        _memberService = memberService;
    }

    // GET api/members
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var members = await _memberService.GetAllMembersAsync();
        return Ok(members);
    }

    // POST api/members
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] MemberRequestDto dto)
    {
        var member = await _memberService.AddMemberAsync(dto);
        return Ok(member);
    }
}
