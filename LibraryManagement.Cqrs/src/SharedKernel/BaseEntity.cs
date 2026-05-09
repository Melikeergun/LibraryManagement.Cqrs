// src/SharedKernel/BaseEntity.cs
//
// Tüm Domain entity'lerinin türediği temel sınıf.
// Id alanı burada tanımlanır; böylece her entity'de tekrar yazmak gerekmez.
//
// İleride bu sınıfa domain event desteği, audit alanları (CreatedAt, UpdatedAt)
// gibi özellikler eklenebilir. Şimdilik sade tutulmuştur.

namespace SharedKernel;

public abstract class BaseEntity
{
    // Her entity'nin benzersiz kimliği
    public int Id { get; protected set; }
}
