# Sangtuari's Career Compass

An online psychological assessment platform developed for **Sangtuari** to help individuals identify their potential, interests, personality traits, and educational or career paths.

---

## 🚀 Project Overview

Sangtuari's Career Compass is built as a highly scalable and dynamic assessment system designed to cater to three distinct user groups:
1. **Exploration**: Designed for Junior High School students (or equivalent).
2. **Discovery**: Designed for Senior High School students (or equivalent).
3. **Advanced**: Designed for College Students, Job Seekers, and Professionals *(Currently in Development)*.

---

## 🛠️ Tech Stack & Architecture

- **Framework:** ASP.NET Core MVC (.NET 8.0)
- **Database:** PostgreSQL
- **Data Flexibility:** Heavy utilization of PostgreSQL **JSONB** column types to store dynamic psychological test profiles and flexible question structures without breaking the core schema.
- **Security:** Managed credentials utilizing `.NET User Secrets` to ensure enterprise-grade security for client source codes.
- **Optimization:** Implemented `IMemoryCache` and read-only `AsNoTracking()` mechanisms to handle massive simultaneous traffic during school wide test periods.

---

## 📸 Registration Workflow Preview

The initial multi-page onboarding flow adapts fluidly between massive desktop monitors and compact mobile screens:

### 1. Assessment Selection Categories
Users choose their appropriate academic/professional tier. The layout splits into a bold left brand panel and clean center-aligned option cards.

### 2. Basic Information (Biographical Details)
Captures core demographic details. Input fields feature full validation, rounded smooth borders, and focus rings consistent with the brand guide.

### 3. Additional Insights (Dynamic Profiling)
Stores additional personal traits (hobbies, goals, liked/disliked subjects) safely bundled into a structured JSONB object inside the database.

---

## ⚙️ Local Development Setup

### Prerequisites
- Visual Studio 2022
- PostgreSQL Instance & DBeaver (Database Manager)

### Quick Run
1. Clone this repository.
2. Initialize and configure local secrets via Package Manager Console:
   ```powershell
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=sangtuari_career_compass;Username=postgres;Password=YOUR_PASSWORD"
   ```
3. Run the EF Core migrations to generate the database schema:
   ```powershell
   Update-Database
   ```
4. Press `Ctrl + F5` in Visual Studio to spin up the web app.

---
*Note: This documentation will be fully comprehensive upon final project completion and server deployment.*
