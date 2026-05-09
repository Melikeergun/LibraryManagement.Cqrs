// src/Web.Api/Controllers/MembersController.cs

using Application.Common.DTOs;
using Application.Common.Exceptions;
using Application.Members.Commands.AddMember;
using Application.Members.Queries.GetAllMembers;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public MembersController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    // GET api/members
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _dispatcher.SendQueryAsync<List<MemberResponseDto>>(
            new GetAllMembersQuery(), ct);
        return Ok(result);
    }

    // POST api/members
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] MemberRequestDto dto, CancellationToken ct)
    {
        try
        {
            var command = new AddMemberCommand
            {
                FullName = dto.FullName,
                Email = dto.Email
            };

            var result = await _dispatcher.SendCommandAsync<MemberResponseDto>(command, ct);
            return Ok(result);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
