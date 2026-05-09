// src/Application/Members/Queries/GetAllMembers/GetAllMembersQuery.cs

using Application.Common.DTOs;
using Domain.Interfaces;
using SharedKernel;

namespace Application.Members.Queries.GetAllMembers;

public class GetAllMembersQuery : IQuery<List<MemberResponseDto>> { }

public class GetAllMembersQueryHandler : IQueryHandler<GetAllMembersQuery, List<MemberResponseDto>>
{
    private readonly IMemberRepository _memberRepository;

    public GetAllMembersQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<List<MemberResponseDto>> HandleAsync(GetAllMembersQuery query, CancellationToken ct = default)
    {
        var members = await _memberRepository.GetAllAsync(ct);

        return members.Select(m => new MemberResponseDto
        {
            Id = m.Id,
            FullName = m.FullName,
            Email = m.Email
        }).ToList();
    }
}
