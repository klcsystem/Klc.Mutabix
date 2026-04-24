# Klc.Mutabix — E-Mutabakat (E-Reconciliation) SaaS Platform

## Proje Ozeti
Kurumsal e-mutabakat platformu. Firmalar arasi cari hesap ve Ba/Bs mutabakatlarini dijitallestirir. ERP entegrasyonlari (SAP, Logo, Netsis, Parasut) ile otomatik veri cekme ve eslestirme.

## Tech Stack
- **Backend**: .NET 10, Clean Architecture + CQRS (MediatR), EF Core + PostgreSQL, JWT auth, FluentValidation, Serilog
- **Frontend**: React 19, TypeScript 5, Vite 7, TailwindCSS 4, React Router 7, React Query 5, Axios, Recharts, Lucide icons
- **Infra**: Docker Compose, PostgreSQL 16, Redis 7, RabbitMQ 3, MailHog (dev SMTP)

## Mimari
```
src/
  Klc.Mutabix.Domain/           -- Entity, Enum (bagimsiz, referans yok)
  Klc.Mutabix.Application/      -- CQRS Commands/Queries, DTO, Validator
  Klc.Mutabix.Infrastructure/   -- EF Core, Email, ERP adapter, JWT
  Klc.Mutabix.Api/              -- Controller, Middleware, Program.cs
  Klc.Mutabix.Tests.Unit/
  Klc.Mutabix.Tests.Integration/
web/
  src/
    api/          -- Axios client
    components/   -- layout/, ui/, reconciliation/
    contexts/     -- AuthContext, TenantContext
    hooks/        -- Custom React hooks
    i18n/         -- TR/EN translations
    pages/        -- Route components
    types/        -- TypeScript interfaces
    utils/        -- Helpers
```

## Port Atamalari
| Servis | Port |
|--------|------|
| web (nginx) | 1700 |
| api (.NET) | 1701 |
| postgres | 1702 |
| redis | 1703 |
| rabbitmq (amqp) | 1709 |
| rabbitmq (mgmt) | 1710 |
| mailhog (smtp) | 1711 |
| mailhog (ui) | 1712 |

## Gelistirme Komutlari
```bash
# Tum servisleri baslat
docker compose up --build -d

# Sadece altyapi (DB, Redis, RabbitMQ)
docker compose up postgres redis rabbitmq mailhog -d

# Backend build & run (lokal)
cd src && dotnet build && dotnet run --project Klc.Mutabix.Api

# Frontend dev server (lokal)
cd web && npm install && npm run dev

# Testler
cd src && dotnet test
cd web && npm run test
```

## Kurallar
- Entity ID'leri Guid
- Multi-tenancy: TenantId tum domain entity'lerde
- Tum API response'lari camelCase JSON
- Enum'lar string olarak serialize
- Frontend stili Gate.Aml: koyu sidebar, turuncu vurgular, split login
- CRUD islemleri icin Drawer (slide-in panel) patterni
- Coklu adim formlar icin Stepper wizard
- I18n: TR/EN destegi
- Turkcede ASCII karakterler kullan (UTF-8 sorunlari onlemek icin)

## Domain Kavramlari
- **AccountReconciliation** — Cari hesap mutabakati (borc/alacak, donem bazli)
- **BaBsReconciliation** — Ba/Bs form mutabakati (aylik, adet/tutar)
- **CurrencyAccount** — Cari hesap (karsi taraf firma)
- **Company** — Firma (multi-tenant)
- **ErpConnection** — ERP baglanti bilgileri (SAP/Logo/Netsis/Parasut)

## Is Akisi
1. Mutabakat olustur (Draft) → detay kalemleri ekle
2. Karsi tarafa email gonder → Status: Sent
3. Karsi taraf Guid link ile acar → Status: Read
4. Onay/red verir → Status: Approved/Rejected
5. Sonuc kaydedilir, bildirim gonderilir
