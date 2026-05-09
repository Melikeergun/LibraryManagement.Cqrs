# 📚 LibraryManagement.Cqrs

> **CQRS + Clean Architecture** ile geliştirilmiş Kütüphane Yönetim Sistemi  
> ASP.NET Core 8 · Entity Framework Core · SQLite · Swagger

Bu proje, **LibraryManagement.Crud** projesiyle birebir aynı işlevleri yerine getiren fakat
**CQRS (Command Query Responsibility Segregation)** ve **Clean Architecture** mimarisiyle yazılmış karşılaştırma projesidir.
Derste iki projeyi yan yana göstererek CQRS'in ne kazandırdığını somut kod üzerinden anlatmak amacıyla hazırlanmıştır.

---

## 🏗️ Mimari

```
src/
├── SharedKernel/       → ICommand, IQuery, IDispatcher, BaseEntity
├── Domain/             → Entity'ler, İş Kuralları, Repository Arayüzleri
├── Application/        → CQRS Command & Query Handler'ları
├── Infrastructure/     → EF Core, SQLite, Repository Implementasyonları
└── Web.Api/            → Controller'lar, Swagger, Program.cs
```

### Katman Bağımlılığı

```
Web.Api
  └─► Application
        └─► Domain
              └─► SharedKernel
Infrastructure
  └─► Domain
```

> **Altın Kural:** Domain hiçbir dış katmanı tanımaz. EF Core, ASP.NET, hiçbir NuGet paketi yoktur. Saf C#.

---

## ⚡ CQRS Nedir?

| | CRUD | CQRS |
|---|---|---|
| Okuma/Yazma | Aynı servis içinde | Tamamen ayrı Handler'larda |
| İş Kuralı | Service metodu içinde | Domain Entity içinde |
| Veritabanı Erişimi | DbContext'e direkt | Repository arayüzü üzerinden |
| Test Edilebilirlik | Servis tümü başlatılmalı | Her Handler bağımsız test edilebilir |
| Yeni İşlem Eklemek | Var olan servisi şişirir | Yeni Handler — mevcut hiçbir şeye dokunmaz |

### Command → Yazma
```
HTTP Request
  → Controller  (Command oluşturur)
  → Dispatcher  (doğru Handler'ı bulur)
  → CommandHandler  (iş kurallarını uygular)
  → Domain Entity  (kural entity içinde)
  → Repository  → Database
```

### Query → Okuma
```
HTTP Request
  → Controller  (Query oluşturur)
  → Dispatcher  (doğru Handler'ı bulur)
  → QueryHandler  (sadece okur, hiçbir şeyi değiştirmez)
  → Repository  → Database
```

---

## 🗂️ Klasör Yapısı

```
LibraryManagement.Cqrs/
├── src/
│   ├── SharedKernel/
│   │   ├── ICqrs.cs              ← ICommand, IQuery, ICommandHandler, IQueryHandler
│   │   ├── Dispatcher.cs         ← Mini MediatR — Handler'ı bulup çalıştırır
│   │   └── BaseEntity.cs         ← Tüm entity'lerin base sınıfı
│   │
│   ├── Domain/
│   │   ├── Entities/
│   │   │   ├── Book.cs           ← DecreaseCopy(), IncreaseCopy(), Update()
│   │   │   ├── Member.cs         ← Create() factory method
│   │   │   └── Loan.cs           ← Return(), IsActive(), IsOverdue()
│   │   ├── Exceptions/
│   │   │   └── DomainException.cs
│   │   └── Interfaces/
│   │       └── IRepositories.cs  ← IBookRepository, IMemberRepository, ILoanRepository
│   │
│   ├── Application/
│   │   ├── Common/
│   │   │   ├── DTOs/             ← BookResponseDto, LoanResponseDto, ...
│   │   │   └── Exceptions/       ← NotFoundException
│   │   ├── Books/
│   │   │   ├── Commands/
│   │   │   │   ├── AddBook/      ← AddBookCommand + AddBookCommandHandler
│   │   │   │   ├── UpdateBook/   ← UpdateBookCommand + UpdateBookCommandHandler
│   │   │   │   └── DeleteBook/   ← DeleteBookCommand + DeleteBookCommandHandler
│   │   │   └── Queries/
│   │   │       ├── GetAllBooks/  ← GetAllBooksQuery + GetAllBooksQueryHandler
│   │   │       └── GetBookById/  ← GetBookByIdQuery + GetBookByIdQueryHandler
│   │   ├── Members/
│   │   │   ├── Commands/AddMember/
│   │   │   └── Queries/GetAllMembers/
│   │   ├── Loans/
│   │   │   ├── Commands/
│   │   │   │   ├── LoanBook/     ← LoanBookCommand + LoanBookCommandHandler
│   │   │   │   └── ReturnBook/   ← ReturnBookCommand + ReturnBookCommandHandler
│   │   │   └── Queries/
│   │   │       └── LoanQueries.cs ← GetActiveLoans + GetOverdueLoans
│   │   └── DependencyInjection.cs ← Tüm Handler kayıtları
│   │
│   ├── Infrastructure/
│   │   ├── Persistence/
│   │   │   ├── AppDbContext.cs
│   │   │   ├── Configurations/   ← Fluent API tablo konfigürasyonları
│   │   │   └── Repositories/     ← BookRepository, MemberRepository, LoanRepository
│   │   └── DependencyInjection.cs
│   │
│   └── Web.Api/
│       ├── Controllers/
│       │   ├── BooksController.cs
│       │   ├── MembersController.cs
│       │   └── LoansController.cs
│       ├── Program.cs
│       └── appsettings.json
└── LibraryManagement.Cqrs.sln
```

---

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Adımlar

```bash
# 1. Repoyu klonla
git clone https://github.com/Melikeergun/LibraryManagement.Cqrs.git
cd LibraryManagement.Cqrs

# 2. Bağımlılıkları yükle
dotnet restore

# 3. Migration oluştur
dotnet ef migrations add InitialCreate --project src/Infrastructure --startup-project src/Web.Api

# 4. Veritabanını oluştur
dotnet ef database update --project src/Infrastructure --startup-project src/Web.Api

# 5. Projeyi çalıştır
cd src/Web.Api
dotnet run
```

### Swagger
Proje ayağa kalktıktan sonra:
```
https://localhost:{PORT}/swagger
```

---

## 📡 API Endpoint'leri

### Kitaplar
| Method | Endpoint | Açıklama | Handler |
|--------|----------|----------|---------|
| `POST` | `/api/books` | Kitap ekle | `AddBookCommandHandler` |
| `GET` | `/api/books` | Tüm kitaplar | `GetAllBooksQueryHandler` |
| `GET` | `/api/books/{id}` | Kitap detayı | `GetBookByIdQueryHandler` |
| `PUT` | `/api/books/{id}` | Kitap güncelle | `UpdateBookCommandHandler` |
| `DELETE` | `/api/books/{id}` | Kitap sil | `DeleteBookCommandHandler` |

### Üyeler
| Method | Endpoint | Açıklama | Handler |
|--------|----------|----------|---------|
| `POST` | `/api/members` | Üye ekle | `AddMemberCommandHandler` |
| `GET` | `/api/members` | Tüm üyeler | `GetAllMembersQueryHandler` |

### Ödünç İşlemleri
| Method | Endpoint | Açıklama | Handler |
|--------|----------|----------|---------|
| `POST` | `/api/loans` | Kitap ödünç ver | `LoanBookCommandHandler` |
| `PUT` | `/api/loans/{id}/return` | Kitap iade et | `ReturnBookCommandHandler` |
| `GET` | `/api/loans/active` | Aktif ödünçler | `GetActiveLoansQueryHandler` |
| `GET` | `/api/loans/overdue` | Geciken kitaplar | `GetOverdueLoansQueryHandler` |

---

## 🧠 Domain Kuralları

| Kural | Nerede Uygulanıyor |
|-------|-------------------|
| AvailableCopies 0 ise ödünç verilemez | `Book.DecreaseCopy()` |
| Ödünç verilince AvailableCopies azalır | `Book.DecreaseCopy()` |
| İade edilince AvailableCopies artar | `Book.IncreaseCopy()` |
| İade edilince ReturnDate doldurulur | `Loan.Return()` |
| Aktif ödünçte olan kitap silinemez | `DeleteBookCommandHandler` |
| DueDate geçmişse gecikmiş sayılır | `Loan.IsOverdue()` |
| DueDate bugünden ileri olmalı | `Loan.Create()` |
| Email formatı geçerli olmalı | `Member.Create()` |

---

## 🔄 CRUD Projesiyle Karşılaştırma

Bu proje **LibraryManagement.Crud** projesiyle birlikte kullanılmak üzere tasarlanmıştır.

```
CRUD:  BookService.AddBookAsync()      → aynı sınıfta listeleme de var
CQRS:  AddBookCommandHandler           → sadece ekleme bilir

CRUD:  LoanService.GetOverdueLoans()  → aynı sınıfta ödünç verme de var
CQRS:  GetOverdueLoansQueryHandler    → sadece okur, yan etki sıfır
```

---

## 🛠️ Kullanılan Teknolojiler

- **ASP.NET Core 8** — Web API
- **Entity Framework Core 8** — ORM
- **SQLite** — Veritabanı
- **Swagger / Swashbuckle** — API dokümantasyonu
- **Clean Architecture** — Katman mimarisi
- **CQRS** — MediatR kullanılmadan, Dispatcher elle yazıldı

---

## 👩‍💻 Geliştirici

**Melike Ergun**  
GitHub: [@Melikeergun](https://github.com/Melikeergun)
