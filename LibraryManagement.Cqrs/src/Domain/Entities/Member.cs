// src/Domain/Entities/Member.cs
//
// Kütüphane üyesini temsil eden entity.
// Üye oluşturulurken email formatı gibi basit doğrulama burada yapılır.

using SharedKernel;
using Domain.Exceptions;

namespace Domain.Entities;

public class Member : BaseEntity
{
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    protected Member() { }

    public static Member Create(string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Üye adı boş olamaz.");

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new DomainException("Geçerli bir e-posta adresi giriniz.");

        return new Member
        {
            FullName = fullName,
            Email = email
        };
    }
}
