// src/Domain/Exceptions/DomainException.cs
//
// Domain katmanında ihlal edilen iş kuralları bu exception ile fırlatılır.
// Örneğin: müsait kopya yokken ödünç verme girişimi.
//
// Neden özel bir exception?
//   Web.Api katmanı bu exception tipini yakalayıp 400 Bad Request
//   döndürebilir. System.Exception ile karıştırılmaz, kasıtlı bir
//   iş kuralı ihlali olduğu net belli olur.

namespace Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}
