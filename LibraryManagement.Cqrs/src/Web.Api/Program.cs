// src/Web.Api/Program.cs
//
// ═══════════════════════════════════════════════════════════════
// UYGULAMA GİRİŞ NOKTASI
// ═══════════════════════════════════════════════════════════════
// Program.cs artık çok temiz.
// Her katman kendi bağımlılıklarını DependencyInjection
// extension metoduyla yönetiyor.
//
// CRUD projesiyle karşılaştır:
//   CRUD: tüm service kayıtları buradaydı
//   CQRS: sadece iki satır — AddApplication + AddInfrastructure
// ═══════════════════════════════════════════════════════════════

using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ── Katmanları kaydet ─────────────────────────────────────────────────────────

// Application: Handler'lar ve Dispatcher
builder.Services.AddApplication();

// Infrastructure: DbContext ve Repository'ler
builder.Services.AddInfrastructure(builder.Configuration);

// ── Web altyapısı ─────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "LibraryManagement.Cqrs API",
        Version = "v1",
        Description = "Clean Architecture + CQRS ile Kütüphane Yönetim Sistemi"
    });
});

// ── Uygulama ──────────────────────────────────────────────────────────────────
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

app.Run();
