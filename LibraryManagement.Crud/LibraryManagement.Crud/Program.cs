// LibraryManagement.Crud/Program.cs

using LibraryManagement.Crud.Data;
using LibraryManagement.Crud.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Servisler ────────────────────────────────────────────────────────────────

// SQLite veritabanı bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=library.db"));

// Servislerimizi DI container'a ekliyoruz
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<LoanService>();

builder.Services.AddControllers();

// Swagger ayarları
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "LibraryManagement.Crud API",
        Version = "v1",
        Description = "Klasik CRUD mimarisiyle hazırlanmış Kütüphane Yönetim Sistemi"
    });
});

// ── Uygulama ─────────────────────────────────────────────────────────────────

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
