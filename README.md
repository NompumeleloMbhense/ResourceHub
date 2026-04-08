# ResourceHub API

ResourceHub is a RESTful API built with **ASP.NET Core** for managing resources and bookings.  
It allows users to create, update, and manage resources (e.g. rooms, equipment) and schedule bookings while preventing conflicts.

---

## Features

- Create, update, and delete resources  
- Book resources for specific time slots  
- Prevent booking conflicts (no overlapping bookings)  
- Input validation using **FluentValidation**  
- Object mapping using **AutoMapper**  
- Global error handling middleware  
- Swagger UI for API testing  
- Clean architecture (Controllers → Services → Domain)

---

## Tech Stack

- **.NET 10 / ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **AutoMapper**
- **FluentValidation**
- **Swagger (Swashbuckle)**

---

## Project Structure

ResourceHub/
│
├── ResourceHub.Api
│ ├── Controllers
│ ├── DTOs
│ ├── Mappings
│ ├── Middleware
│ ├── Validation
│
├── ResourceHub.Core
│ ├── Entities
│ ├── Interfaces
│ ├── Exceptions
│
├── ResourceHub.Infrastructure
│ ├── Persistence
│ ├── Services

---

## Getting Started

### 1. Clone the repository
git clone https://github.com/your-username/resourcehub.git
cd resourcehub

### 2. Configure Database
Update your appsettings.json:
"ConnectionStrings": {
  "ResourceHubConnection": "Server=YOUR_SERVER;Database=ResourceHubDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

### 3. Run Migrations
dotnet ef database update

### 4. Run the API
dotnet run

### 5. Open Swagger
https://localhost:{port}/swagger

---

## API Endpoints

**Resources**

| Method | Endpoint           | Description        |
| ------ | ------------------ | ------------------ |
| GET    | /api/resource      | Get all resources  |
| GET    | /api/resource/{id} | Get resource by ID |
| POST   | /api/resource      | Create resource    |
| PUT    | /api/resource/{id} | Update resource    |
| DELETE | /api/resource/{id} | Delete resource    |

**Bookings**

| Method | Endpoint                           | Description                 |
| ------ | ---------------------------------- | --------------------------- |
| GET    | /api/booking                       | Get all bookings            |
| GET    | /api/booking/{id}                  | Get booking by ID           |
| GET    | /api/booking/resource/{resourceId} | Get bookings for a resource |
| POST   | /api/booking                       | Create booking              |
| PUT    | /api/booking/{id}                  | Update booking              |
| DELETE | /api/booking/{id}                  | Delete booking              |


---

## Business Rules

- A resource cannot be double-booked for overlapping time periods
- A resource cannot be deleted if it has existing bookings
- Start time must be before end time
- Required fields are validated using FluentValidation

---

## Example Request

**Create Resource**

{
  "name": "Conference Room A",
  "description": "Main meeting room",
  "location": "First Floor",
  "capacity": 10
}

**Create Booking**
{
  "resourceId": 1,
  "startTime": "2026-04-10T09:00:00",
  "endTime": "2026-04-10T10:00:00",
  "bookedBy": "Nompumelelo",
  "purpose": "Team Meeting"
}

---

## Error Handling

**The API uses global exception handling middleware.**

Example Error Response:
{
  "message": "This resource is already booked for the selected time."
}

Common Status Codes:

| Code | Meaning                                        |
| ---- | ---------------------------------------------- |
| 400  | Bad Request (validation/business rule failure) |
| 404  | Resource not found                             |
| 409  | Conflict (e.g. booking overlap)                |
| 500  | Internal server error                          |

---

## Validation
Validation is handled using FluentValidation:

- Required fields enforced
- Date comparisons (StartTime < EndTime)
- String length limits

---

## Architecture Highlights

- DTOs separate API from domain models
- AutoMapper reduces manual mapping
- Services layer handles business logic
- Entities enforce domain rules
- Middleware handles errors globally

---

## Future Improvements

- Soft delete for resources
- Authentication & authorization
- Pagination & filtering
- Logging (Serilog)
- Unit & integration tests

---

## Author
Nompumelelo
Software Developer focused on .NET and backend systems

---

## License
This project is for learning and portfolio purposes.
