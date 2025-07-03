# House Broker Application MVP - ASP.NET Core Web API

This project is a **Minimum Viable Product (MVP) for a House Broker Application using the .NET, MSSQL for the database, and adhering to Clean Architecture principles.**. 
The application supports authentication, listing management, commission calculation, search/filter features, and basic unit testing.

---

## Features

### User Authentication
- ASP.NET Core Identity-based authentication
- Role-based access (Broker / Seeker)
- JWT Token-based security

### Property Listings
- Full CRUD operations
- Property details: type, location, price, features, description, and images
- Broker-only visibility for commission
- API ready for third-party consumption

### Search and Filter
- Filter listings by:
  - **Location**
  - **Price range**
  - **Property type**
- Detailed listing view including:
  - **Images**
  - **Features & Description**

### Unit Testing
- Tests included for:
  - Listing service logic

## Technologies Used

- [.NET 8 Web API](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [MSSQL Server](https://www.microsoft.com/en-us/sql-server)
- [AutoMapper](https://automapper.org/)
- [JWT Bearer Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt)
- [xUnit](https://xunit.net/) + [Moq](https://github.com/moq)

---

## Setup Instructions

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- Optional: [Postman](https://www.postman.com/) or [Swagger UI](http://localhost:5000/swagger)

### 1. Clone the Repository

```bash
git clone https://github.com/RajinMaharjan/HouseBrokerApplication.git
```
```bash
cd HouseBrokerApplication
```
```bash
dotnet run
```
### 2. Update Configuration
- Update appsettings.json
```
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=HouseBrokerDb;Trusted_Connection=True;"
},
"JwtConfig": {
  "ValidIssuer": "HouseBrokerApp",
  "ValidAudience": "HouseBrokerUsers",
  "Secret": "WXJSAMFKLSTEHSYOVASDWQ",
  "ExpiresIn": 10
}
```


