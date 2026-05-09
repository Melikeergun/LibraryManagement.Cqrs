// src/SharedKernel/IDispatcher.cs  &  Dispatcher.cs
//
// ═══════════════════════════════════════════════════════════════
// DISPATCHER (Mini MediatR)
// ═══════════════════════════════════════════════════════════════
// Dispatcher, Controller ile Handler arasındaki köprüdür.
//
// Controller bir Command veya Query oluşturur ve Dispatcher'a gönderir.
// Dispatcher, DI container'a sorar: "Bu tip için kayıtlı Handler kim?"
// Bulduğu Handler'ı çalıştırır ve sonucu döndürür.
//
// MediatR da temelde bunu yapar; burada elle, şeffaf şekilde yazılmıştır.
// Amacımız: soyutlamanın arkasını görmek.
// ═══════════════════════════════════════════════════════════════

namespace SharedKernel;

// Dispatcher'ın dışarıya sunduğu sözleşme
public interface IDispatcher
{
    Task<TResult> SendCommandAsync<TResult>(ICommand<TResult> command, CancellationToken ct = default);
    Task<TResult> SendQueryAsync<TResult>(IQuery<TResult> query, CancellationToken ct = default);
}

// Dispatcher'ın gerçek implementasyonu
public class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Command'ı alır, doğru CommandHandler'ı bulur, çalıştırır.
    /// </summary>
    public Task<TResult> SendCommandAsync<TResult>(ICommand<TResult> command, CancellationToken ct = default)
    {
        // Reflection ile "ICommandHandler<AddBookCommand, BookResponseDto>" tipini oluştur
        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(command.GetType(), typeof(TResult));

        // DI container'dan bu tipe kayıtlı Handler'ı çek
        dynamic handler = _serviceProvider.GetRequiredService(handlerType);

        // Handler'ı çalıştır
        return handler.HandleAsync((dynamic)command, ct);
    }

    /// <summary>
    /// Query'yi alır, doğru QueryHandler'ı bulur, çalıştırır.
    /// </summary>
    public Task<TResult> SendQueryAsync<TResult>(IQuery<TResult> query, CancellationToken ct = default)
    {
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResult));

        dynamic handler = _serviceProvider.GetRequiredService(handlerType);

        return handler.HandleAsync((dynamic)query, ct);
    }
}
