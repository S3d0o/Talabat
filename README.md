# 📦 Talabat E‑Commerce Backend API

A **production‑grade E‑Commerce backend** inspired by Talabat, built with **ASP.NET Core** and designed using **Onion Architecture** and **real‑world backend engineering patterns**.

This project is not a CRUD demo. It focuses on **scalability, performance, security, and correctness** — the same concerns faced in real production systems.

---

## 🧠 Project Philosophy

This backend was built with the mindset of:

* Protecting **business logic** from infrastructure concerns
* Designing for **change, scale, and testing**
* Solving **real backend problems** (caching, concurrency, token theft, invalidation)
* Writing code that is **interview‑ready and production‑oriented**

Every layer, abstraction, and pattern exists for a reason.

---

## 🏗️ Architecture Overview (Onion Architecture)

Dependencies flow **inward only**:

```
Presentation (API)
     ↓
Application / Services
     ↓
Core (Domain + Contracts)
     ↓
Infrastructure (Persistence, Identity, Redis)
```

* Core contains **pure business rules**
* Infrastructure details are fully replaceable
* Presentation is thin and delegates logic

---

## 🧱 Solution Structure

```
Talabat
│
├── Core
│   ├── Domain
│   │   ├── Entities
│   │   ├── Contracts
│   │   ├── Exceptions
│   │   └── Global abstractions
│   │
│   ├── Service.Abstraction
│   │   └── Service contracts
│   │
│   └── Services
│       ├── Business logic implementations
│       ├── Specifications
│       ├── Mapping profiles
│       └── Shared helpers
│
├── Infrastructure
│   ├── Persistence
│   │   ├── App database context
│   │   ├── Identity database context
│   │   ├── Repository implementations
│   │   └── EF Core helpers
│   │
│   └── Presentation
│       ├── API controllers
│       ├── Custom attributes (Redis cache)
│       └── Filters & API helpers
│
├── Shared
│   ├── DTOs (Product, Order, Basket, Identity)
│   ├── Parameters (pagination, filtering)
│   ├── Error models
│   └── Helper utilities
│
└── E‑Commerce (API Host)
    ├── Middlewares
    ├── Extensions
    ├── Factories
    ├── Program.cs
    └── Configuration files
```

---

## 🚀 Core Features

### 🛒 E‑Commerce Domain

* Products, Orders, Baskets, and Customers
* Strongly modeled domain entities
* DTO‑based communication (no entity leakage)
* Pagination, filtering, and sorting

---

### ⚡ High‑Performance Caching (Redis)

The project uses **Redis as a distributed cache**, integrated deeply into the API design:

* Custom **Redis cache attribute** applied at **endpoint level**
* **Cache invalidation on write operations** (Create / Update / Delete)
* Prevents redundant database hits
* Designed to be **safe for concurrent requests**

This mirrors how caching is implemented in real production APIs — not simple in‑memory caching.

---

### 🔐 Advanced Authentication & Security

Authentication is implemented with **enterprise‑level security considerations**:

* JWT access tokens
* Secure refresh tokens
* **Refresh token rotation**
* **Token reuse (theft) detection**
* Automatic **token revocation on suspicious reuse**
* Protection against replay attacks

This goes far beyond basic JWT tutorials and reflects **real‑world identity flows**.

---

### 🧠 Business Logic Layer (Services)

* All business rules live inside the Services layer
* Controllers remain thin and orchestration‑only
* Clear service abstractions for testing and extension
* No direct EF Core usage outside Infrastructure

---

### 🗂️ Data Access Layer

* Repository Pattern
* Specification Pattern for complex queries
* Separate Identity & Application contexts
* Fully abstraction‑driven persistence layer

Specifications enable:

* Reusable query logic
* Clean filtering & sorting
* Query composability

---

### ⚠️ Global Error Handling

* Centralized exception handling middleware
* Consistent API error responses
* Custom error models
* No unhandled exceptions leaking to clients
  
---

### 💳 Payments & Stripe Integration

The project includes a **real-world Stripe payment integration** designed with reliability and consistency in mind:

- Stripe **Payment Intents API**
- Secure webhook handling
- **Signature verification** to prevent spoofed requests
- Idempotent payment processing
- Backend-driven payment state updates
- Separation between payment logic and domain logic
  
---

### 💡 Payment Flow (Stripe)

1. Client requests payment intent from API
2. Backend creates Stripe PaymentIntent
3. Client confirms payment using Stripe SDK
4. Stripe sends webhook event
5. Backend verifies webhook signature
6. Order payment status is updated safely
7. Duplicate events are ignored using idempotency

This ensures the system remains consistent even under retries or network failures.

The payment flow is designed to be **safe against duplicate events**, network retries, and inconsistent client states — matching how payments are handled in production systems.

---

## 🛠️ Tech Stack

* **ASP.NET Core**
* **C#**
* **Entity Framework Core**
* **Redis (Distributed Caching)**
* **Stripe API (Payments & Webhooks)**
* **ASP.NET Identity**
* **AutoMapper**
* **Onion Architecture**
* **Repository & Specification Patterns**

---

## ⚙️ Getting Started

### Prerequisites

* .NET SDK (latest LTS)
* SQL Server (or compatible provider)
* Redis server

### Run the Project

```bash
git clone https://github.com/your-username/Talabat.git
cd Talabat
dotnet restore
dotnet run --project E-Commerce
```

The API will be available at:

```
https://localhost:5001
```

---

## 🔧 Configuration

Update connection strings and Redis settings in:

* `appsettings.json`
* `appsettings.Development.json`

---

## 🧩 Why This Project Stands Out

This project demonstrates:

* Real **backend system design**, not tutorials
* Performance optimization with Redis
* Secure authentication with theft detection
* Clean separation of concerns
* Enterprise‑style architecture decisions
* Real payment processing using Stripe with secure webhooks and idempotency

It’s designed to be:

* Easy to extend
* Easy to test
* Safe under load
* Interview‑ready

---

## 👨‍💻 Author

**Saad Mohamed**
Backend Developer – ASP.NET Core
Onion Architecture • C# • SQL • Distributed Systems
