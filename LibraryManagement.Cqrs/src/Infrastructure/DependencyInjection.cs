// src/Infrastructure/DependencyInjection.cs
//
// Infrastructure katmanının bağımlılıklarını DI container'a kaydeden
// extension metot. Program.cs'i temiz tutar.
//
// Kullanım: builder.Services.AddInfrastructure(builder.Configuration);

using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // EF Core + SQLite
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=library.db"));

        // Repository kayıtları:
        // "IBookRepository istenirse BookRepository ver"
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();

        return services;
    }
}
