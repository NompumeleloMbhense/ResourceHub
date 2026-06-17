# ResourceHub API

ResourceHub is a full-stack resource booking and management system built with **Blazor WebAssembly, 
ASP.NET Core 10 Web API and Entity Framework Core**.

The application enables organizations to manage shared resources such as meeting rooms, equipment and workspaces
while providing a streamlined booking experience with authentication, filtering, dashboards and booking management capabilities.

---

## Key Features

**Resource Management**
- Create, edit, view, and delete resources
- Track resource availability
- Capacity management
- Search resources by name and location
- Filter resources by availability and capacity range
- Paginated resource listings

**Booking Management**
- Create, edit, view, and delete bookings
- Move bookings between resources
- View booking history for individual resources
- Search bookings by keyword or user
- Filter bookings by date range
- Paginated booking listings

**Dashboard**
- Total resources overview
- Available resources overview
- Total bookings overview
- Upcoming bookings overview
- Upcoming bookings table for quick visibility into scheduled reservations

**Authentication & Security**
- JWT-based authentication
- Protected routes using authorization
- Secure API communication between client and server

---

## Tech Stack

- **C#**
- **.NET 10**
- **Entity Framework Core**
- **Blazor WebAssembly**
- **SQL Server**
- **AutoMapper**
- **HTML & CSS**
- **JWT Authentication**

---

## System Architecture

The application follows a layered architecture across multiple projects:

**ResourceHub.Api (Backend)**
ASP.NET Core Web API responsible for:

- Business logic
- Validation
- Repository implementations
- Entity Framework Core database access
- Authentication and authorization

**ResourceHub.Client (Frontend)**
Blazor WebAssembly application responsible for:

- User interface
- API communication
- State management
- Resource and booking management workflows

**ResourceHub.Core**
Contains:

- Domain entities
- Repository interfaces
- Query parameter models
- Shared business contracts

**ResourceHub.Shared**
Contains:

- DTOs (Data Transfer Objects)
- Form models
- Shared validation models

**ResourceHub.Infrastructure**
Responsible for:

- Data persistence
- Repository implementations
- Database configuration
- Entity Framework Core integration

---

## Getting Started

**Prerequisites**
- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or VS Code

**Clone Repository**

    git clone https://github.com/NompumeleloMbhense/ResourceHub.git

**Navigate To Project**

    cd ResourceHub

**Update Database Connection**

Update the connection string in:

    appsettings.json

example:

    "ConnectionStrings": {
          "DefaultConnection": "YOUR_CONNECTION_STRING_HERE"
    }

**Apply Migrations**

    dotnet ef database update
    
**Run API**

    dotnet run --project ResourceHub.Api

**Run Client**

    dotnet run --project ResourceHub.Client

---

## Challenges and Solutions

**1. Booking Validation**

Challenge

Users could create bookings where the end time occurred before the start time, resulting in invalid reservations.

Solution

Implemented both client-side and server-side validation to ensure:
- End time must be later than start time
- Invalid bookings are rejected before being saved
- Clear validation messages are displayed to users

**2. Error Handling & Standardised Exception Design**

Challenge

The API initially handled exceptions using a large switch statement in middleware to map different exception types to HTTP status codes. As the number of domain exceptions grew, this approach became harder to maintain and violated the Open/Closed Principle.

Solution

Refactored the error-handling system to use a standardised base exception pattern, removing the need for switch-based mapping:
- Introduced a base exception class (e.g. AppException) containing a reusable HTTP status code property
- All domain-specific exceptions (e.g. BookingConflictException, ResourceNotFoundException) inherit from this base class
- Updated global exception middleware to handle all custom exceptions in a single unified block
- Eliminated large switch statements for cleaner and more maintainable mapping
- Ensured each exception is self-describing and carries its own HTTP status responsibility


**3. Booking Relocation Feature**

Challenge

Users needed a way to move existing bookings to a different resource without deleting and recreating them.

Solution

Implemented a booking relocation workflow:
- Users can select an existing booking
- Choose a new resource
- Update the booking while preserving booking details

---

## Future Improvements

- Future Improvements
- Role-based authorization
- Booking conflict calendar view
- Email notifications
- Resource images
- Analytics dashboard
- Dark mode
- Database auditing
- Real-time booking updates with SignalR

---

## Author

**Nompumelelo Mbhense**
Software Developer focused on the Microsoft ecosystem, building projects with C#, .NET, ASP.NET Core, Blazor and SQL Server.

---

## License
This project is licensed under the MIT License.
