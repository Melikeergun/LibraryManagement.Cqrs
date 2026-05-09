// src/SharedKernel/ICommand.cs
//
// ═══════════════════════════════════════════════════════════════
// SHARED KERNEL KATMANI
// ═══════════════════════════════════════════════════════════════
// SharedKernel; tüm katmanların kullandığı ortak sözleşmeleri
// (interface, base class, marker interface) barındırır.
//
// Bu dosya CQRS'in temel yapı taşlarını tanımlar:
//   ICommand    → "Bir şeyi değiştir" talebi
//   IQuery      → "Bir şeyi oku" talebi
//   ICommandHandler → Command'ı işleyen sınıf sözleşmesi
//   IQueryHandler   → Query'yi işleyen sınıf sözleşmesi
//
// Neden ayrı bir katman?
//   Bu arayüzler hem Application hem Domain hem Web.Api
//   tarafından kullanılır. Ortak bir yerde tutmak döngüsel
//   bağımlılığı (circular dependency) önler.
// ═══════════════════════════════════════════════════════════════

namespace SharedKernel;

// ── COMMAND ──────────────────────────────────────────────────────────────────

/// <summary>
/// Tüm Command'ların implement etmesi gereken işaretleyici arayüz.
/// TResult: Handler'ın döndüreceği sonucun tipi.
///
/// Command = sisteme bir değişiklik yapmak için gönderilen talep.
/// Örnek: AddBookCommand, LoanBookCommand, ReturnBookCommand
/// </summary>
public interface ICommand<TResult> { }

// ── QUERY ─────────────────────────────────────────────────────────────────────

/// <summary>
/// Tüm Query'lerin implement etmesi gereken işaretleyici arayüz.
/// TResult: Handler'ın döndüreceği okuma sonucunun tipi.
///
/// Query = sistemden veri okumak için gönderilen talep.
/// Yan etki ÜRETMEZ — kaç kez çağırılırsa çağırılsın veriyi değiştirmez.
/// Örnek: GetAllBooksQuery, GetOverdueLoansQuery
/// </summary>
public interface IQuery<TResult> { }

// ── COMMAND HANDLER ───────────────────────────────────────────────────────────

/// <summary>
/// Belirli bir Command tipini işleyen Handler sınıflarının sözleşmesi.
///
/// Her Command için TAM OLARAK BİR Handler bulunur.
/// Handler; iş kurallarını uygular, repository'yi çağırır, sonucu döndürür.
/// </summary>
public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

// ── QUERY HANDLER ─────────────────────────────────────────────────────────────

/// <summary>
/// Belirli bir Query tipini işleyen Handler sınıflarının sözleşmesi.
///
/// QueryHandler ASLA veri değiştirmez — sadece okur ve döndürür.
/// Bu sayede okuma tarafı bağımsız olarak optimize edilebilir.
/// </summary>
public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
