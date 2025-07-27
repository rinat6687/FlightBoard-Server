# flight_board_server

A flight management system, including a client side (React) and a server side (ASP.NET Core). The system allows flights to be displayed, added, deleted, filtered and updated in real time using SignalR.

---

## 🚀 Installation and running instructions

### ✅ Server side (Backend - ASP.NET Core)

1. Open the server folder:

```
cd FlightBoardApp/flight_board_server

```

2. Restore the dependencies:

```
dotnet restore
```

3. Run the server:

```
dotnet run
```

> The server will listen at: `https://localhost:5001` or `http://localhost:5000`

---

## 🧱 Architectural explanation

- **Backend**:
- ASP.NET Core Web API.
- Layered architecture: Domain, Application, Infrastructure, API.
- Entity Framework Core for data management.
- AutoMapper for converting entities to DTO.

- **Communication**:
- REST API between the frontend and backend.
- SignalR for live broadcasts from the server to the client (such as adding a flight in real time).

---

## 📦 Third-party packages

### Backend:

- `Microsoft.AspNetCore.SignalR`
- `AutoMapper`
- `FluentValidation`
- `EntityFrameworkCore`
- `Swashbuckle.AspNetCore` (for Swagger)

---

## ✨ Project creator

**Rinat skinny**
[rinat6687@gmail.com](mailto:rinat6687@gmail.com)
