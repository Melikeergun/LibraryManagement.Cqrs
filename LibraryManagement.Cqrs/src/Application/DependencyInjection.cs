// src/Application/DependencyInjection.cs
//
// Application katmanının tüm Handler'larını DI container'a kaydeder.
// Her Command/Query için ilgili Handler kayıt edilir.
//
// "Bu kadar çok kayıt mı?" diye sorabilirsin.
// Evet — ama bu aynı zamanda sistemdeki tüm işlemlerin listesi.
// Program.cs'e baktığında sistemin tam kapasitesini görürsün.

using Application.Books.Commands.AddBook;
using Application.Books.Commands.DeleteBook;
using Application.Books.Commands.UpdateBook;
using Application.Books.Queries.GetAllBooks;
using Application.Books.Queries.GetBookById;
using Application.Common.DTOs;
using Application.Loans.Commands.LoanBook;
using Application.Loans.Commands.ReturnBook;
using Application.Loans.Queries.GetActiveLoans;
using Application.Loans.Queries.GetOverdueLoans;
using Application.Members.Commands.AddMember;
using Application.Members.Queries.GetAllMembers;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Dispatcher — Command/Query'leri ilgili Handler'a yönlendirir
        services.AddScoped<IDispatcher, Dispatcher>();

        // ── BOOK COMMAND HANDLERS ─────────────────────────────────────────────
        services.AddScoped<ICommandHandler<AddBookCommand, BookResponseDto>, AddBookCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateBookCommand, BookResponseDto>, UpdateBookCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteBookCommand, bool>, DeleteBookCommandHandler>();

        // ── BOOK QUERY HANDLERS ───────────────────────────────────────────────
        services.AddScoped<IQueryHandler<GetAllBooksQuery, List<BookResponseDto>>, GetAllBooksQueryHandler>();
        services.AddScoped<IQueryHandler<GetBookByIdQuery, BookResponseDto>, GetBookByIdQueryHandler>();

        // ── MEMBER COMMAND HANDLERS ───────────────────────────────────────────
        services.AddScoped<ICommandHandler<AddMemberCommand, MemberResponseDto>, AddMemberCommandHandler>();

        // ── MEMBER QUERY HANDLERS ─────────────────────────────────────────────
        services.AddScoped<IQueryHandler<GetAllMembersQuery, List<MemberResponseDto>>, GetAllMembersQueryHandler>();

        // ── LOAN COMMAND HANDLERS ─────────────────────────────────────────────
        services.AddScoped<ICommandHandler<LoanBookCommand, LoanResponseDto>, LoanBookCommandHandler>();
        services.AddScoped<ICommandHandler<ReturnBookCommand, LoanResponseDto>, ReturnBookCommandHandler>();

        // ── LOAN QUERY HANDLERS ───────────────────────────────────────────────
        services.AddScoped<IQueryHandler<GetActiveLoansQuery, List<LoanResponseDto>>, GetActiveLoansQueryHandler>();
        services.AddScoped<IQueryHandler<GetOverdueLoansQuery, List<LoanResponseDto>>, GetOverdueLoansQueryHandler>();

        return services;
    }
}
