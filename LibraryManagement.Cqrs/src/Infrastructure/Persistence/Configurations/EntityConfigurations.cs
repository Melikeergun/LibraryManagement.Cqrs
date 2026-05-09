// src/Infrastructure/Persistence/Configurations/BookConfiguration.cs
//
// EF Core Fluent API ile tablo konfigürasyonları.
// Entity'nin property'leri private set olduğu için
// bu dosyada HasField veya Property ile EF'e tanıtılır.
//
// Bu ayrım sayesinde Domain entity'si EF Core attribute'larından
// (örn. [Column], [Required]) tamamen temiz kalır.

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(b => b.ISBN)
            .HasMaxLength(20);

        builder.Property(b => b.TotalCopies)
            .IsRequired();

        builder.Property(b => b.AvailableCopies)
            .IsRequired();
    }
}

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(100);
    }
}

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.HasKey(l => l.Id);

        // İlişkiler
        builder.HasOne(l => l.Book)
            .WithMany()
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Member)
            .WithMany()
            .HasForeignKey(l => l.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(l => l.LoanDate).IsRequired();
        builder.Property(l => l.DueDate).IsRequired();
        builder.Property(l => l.ReturnDate).IsRequired(false);
    }
}
