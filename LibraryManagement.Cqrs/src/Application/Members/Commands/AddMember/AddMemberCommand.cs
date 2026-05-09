// src/Application/Members/Commands/AddMember/AddMemberCommand.cs

using Application.Common.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using SharedKernel;

namespace Application.Members.Commands.AddMember;

public class AddMemberCommand : ICommand<MemberResponseDto>
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class AddMemberCommandHandler : ICommandHandler<AddMemberCommand, MemberResponseDto>
{
    private readonly IMemberRepository _memberRepository;

    public AddMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<MemberResponseDto> HandleAsync(AddMemberCommand command, CancellationToken ct = default)
    {
        // Email doğrulaması Member.Create içinde yapılır (Domain kuralı)
        var member = Member.Create(command.FullName, command.Email);

        await _memberRepository.AddAsync(member, ct);

        return new MemberResponseDto
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email
        };
    }
}
