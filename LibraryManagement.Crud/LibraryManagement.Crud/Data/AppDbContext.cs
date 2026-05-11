// LibraryManagement.Crud/Data/AppDbContext.cs

using LibraryManagement.Crud.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Crud.Data;

// Entity Framework Core'un veritabanı bağlamı.
// Tüm tablolar burada tanımlanır.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();
}
