# 🎓 Student Management API

A modern, robust, and scalable **.NET 9 Web API** designed for managing student records. Built with a focus on clean architecture, CQRS pattern, and secure authentication.

---

## 🚀 Features

- **Full CRUD Operations**: Create, Read, Update, and Delete student records.
- **JWT Authentication**: Secure endpoints using JSON Web Tokens.
- **CQRS Pattern**: Implemented using **Cortex Mediator** for clear separation of concerns.
- **Fluent Validation**: Robust input validation for all API requests.
- **Automated Mapping**: Effortless DTO-to-Entity mapping with **Mapster**.
- **Unit Testing**: Comprehensive test suite using **xUnit**, **NSubstitute**, and **Shouldly**.

---

## 🛠️ Tech Stack

- **Framework**: .NET 9.0
- **Database**: SQL Server (EF Core)
- **Mediator Pattern**: Cortex.Mediator
- **Validation**: FluentValidation
- **Mapping**: Mapster
- **Logging**: Serilog
- **Testing**: xUnit, NSubstitute, Shouldly, MockQueryable

---

## 📥 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or LocalDB)

### Setup Instructions

1.  **Clone the Repository**
    ```bash
    git clone <repository-url>
    cd StudentApi
    ```

2.  **Configure Database**
    Open `StudentApi/appsettings.json` and update the `ConnectionStrings:connection` to point to your SQL Server instance.
    ```json
    "ConnectionStrings": {
      "connection": "Server=YOUR_SERVER;Database=StudentApi;Trusted_Connection=True;TrustServerCertificate=True"
    }
    ```

3.  **Apply Migrations**
    Run the following command to create the database and tables:
    ```bash
    dotnet ef database update --project StudentApi
    ```

4.  **Run the Application**
    ```bash
    dotnet run --project StudentApi
    ```
    The API will be available at `https://localhost:7080` (or the port specified in your `launchSettings.json`). You can access the **Swagger UI** at `/swagger`.

---

## 🔐 Authentication

All Student endpoints (except Login) require a Bearer Token.

### Login (Static Credentials)
- **URL**: `/api/Auth/login`
- **Method**: `POST`
- **Body**:
  ```json
  {
    "username": "admin",
    "password": "password123"
  }
  ```
- **Response**: Returns a JWT token to be used in the `Authorization` header as `Bearer <token>`.

---

## 🧪 Running Tests

To execute the unit test suite, run:
```bash
dotnet test
```

---

## 📂 Project Structure

- **StudentApi**: Main Web API project containing Controllers and Features (Handlers, Models, Validators).
- **Core**: Core entities, enums, and domain logic.
- **Infrastructure**: Data access (EF Core), Repositories, and cross-cutting concerns (Middleware, Logging Behaviors).
- **Util**: Utility classes and helper methods.
- **StudentApiTest**: Unit tests for all API features.

---

## 📝 License

This project is licensed under the MIT License.
