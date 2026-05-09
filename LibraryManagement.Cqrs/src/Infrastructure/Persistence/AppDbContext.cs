// src/Infrastructure/Persistence/AppDbContext.cs
//
// ═══════════════════════════════════════════════════════════════
// INFRASTRUCTURE KATMANI — Veritabanı Bağlamı
// ═══════════════════════════════════════════════════════════════
// Infrastructure: teknik detayları barındırır.
//   - Veritabanı (EF Core + SQLite)
//   - Repository implementasyonları
//   - Tablo konfigürasyonları
//
// Domain ve Application katmanları bu katmanı tanımaz.
// Dependency Inversion: Domain arayüz tanımlar (IBookRepository),
// Infrastructure uygular (BookRepository : IBookRepository).
// ═══════════════════════════════════════════════════════════════

using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Her entity için konfigürasyon sınıfını uygula
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new MemberConfiguration());
        modelBuilder.ApplyConfiguration(new LoanConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
