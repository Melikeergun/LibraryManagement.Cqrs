// src/Application/Common/Exceptions/NotFoundException.cs
//
// Bir kaynak bulunamadığında fırlatılır.
// Web.Api bu exception'ı 404 Not Found olarak döndürür.

namespace Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, int id)
        : base($"{entityName} bulunamadı. Id: {id}") { }
}
